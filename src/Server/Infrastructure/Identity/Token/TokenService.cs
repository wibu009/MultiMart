using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MultiMart.Application.Common.Caching;
using MultiMart.Application.Common.Interfaces;
using MultiMart.Application.Identity.Tokens;
using MultiMart.Infrastructure.Auth;
using MultiMart.Infrastructure.Auth.Jwt;
using MultiMart.Infrastructure.Auth.OAuth2;
using MultiMart.Infrastructure.Common.Extensions;
using MultiMart.Infrastructure.Common.Settings;
using MultiMart.Infrastructure.Identity.User;
using MultiMart.Infrastructure.Multitenancy;
using MultiMart.Shared.Authorization;
using MultiMart.Shared.Multitenancy;
using SendGrid.Helpers.Errors.Model;
using UnauthorizedException = MultiMart.Application.Common.Exceptions.UnauthorizedException;

namespace MultiMart.Infrastructure.Identity.Token;

internal class TokenService : ITokenService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IStringLocalizer _t;
    private readonly SecuritySettings _securitySettings;
    private readonly JwtSettings _jwtSettings;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ApplicationTenantInfo? _currentTenant;
    private readonly EncryptionSettings _encryptionSettings;
    private readonly ICurrentUser _currentUser;
    private readonly ICacheService _cacheService;
    private readonly string _refreshTokenCookieName;

    public TokenService(
        UserManager<ApplicationUser> userManager,
        IOptions<JwtSettings> jwtSettings,
        IStringLocalizer<TokenService> t,
        ApplicationTenantInfo? currentTenant,
        IOptions<SecuritySettings> securitySettings,
        IHttpContextAccessor httpContextAccessor,
        IOptions<EncryptionSettings> encryptionSettings,
        ICacheService cacheService,
        ICurrentUser currentUser)
    {
        _userManager = userManager;
        _t = t;
        _jwtSettings = jwtSettings.Value;
        _currentTenant = currentTenant;
        _httpContextAccessor = httpContextAccessor;
        _cacheService = cacheService;
        _currentUser = currentUser;
        _securitySettings = securitySettings.Value;
        _encryptionSettings = encryptionSettings.Value;
        _refreshTokenCookieName = $"refreshToken-{_currentTenant.Id}".Encrypt(_encryptionSettings.Key, _encryptionSettings.IV);
    }

    public async Task<TokenResponse> GetTokenAsync(TokenRequest request, string ipAddress,
        CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.UserNameOrEmail.Trim().Normalize())
                   ?? await _userManager.FindByNameAsync(request.UserNameOrEmail.Trim().Normalize());

        if (string.IsNullOrWhiteSpace(_currentTenant?.Id) || user == null ||
            !await _userManager.CheckPasswordAsync(user, request.Password))
        {
            throw new UnauthorizedException(_t["Authentication Failed."]);
        }

        if (!user.IsActive)
        {
            throw new UnauthorizedException(_t["User Not Active. Please contact the administrator."]);
        }

        if (_securitySettings.RequireConfirmedAccount && !user.EmailConfirmed)
        {
            throw new UnauthorizedException(_t["E-Mail not confirmed."]);
        }

        if (_currentTenant.Id != MultitenancyConstants.Root.Id)
        {
            if (!_currentTenant.IsActive)
            {
                throw new UnauthorizedException(
                    _t["Tenant is not Active. Please contact the Application Administrator."]);
            }

            if (DateTime.UtcNow > _currentTenant.ValidUpto)
            {
                throw new UnauthorizedException(
                    _t["Tenant Validity Has Expired. Please contact the Application Administrator."]);
            }
        }

        string? oldUserRefreshToken = _httpContextAccessor.HttpContext?.Request.Cookies[_refreshTokenCookieName];
        if (!string.IsNullOrEmpty(oldUserRefreshToken))
        {
            await RevokeRefreshToken(oldUserRefreshToken);
        }

        return await GenerateTokensAndUpdateUser(user, ipAddress);
    }

    public async Task<TokenResponse> GetTokenAsync(string? token, string ipAddress, CancellationToken cancellationToken)
    {
        if (_currentTenant == null || string.IsNullOrWhiteSpace(_currentTenant.Id))
        {
            throw new UnauthorizedException(_t["Authentication Failed."]);
        }

        string? decodedState = WebUtility.UrlDecode(token);
        var stateData = decodedState.Contains("#/_=_")
            ? decodedState.Replace("#/_=_", string.Empty).Decrypt<StateData<string>>(_encryptionSettings.Key, _encryptionSettings.IV)
            : decodedState.Decrypt<StateData<string>>(_encryptionSettings.Key, _encryptionSettings.IV);
        string tenantId = stateData.TenantId;
        var expireAt = stateData.ExpireAt;
        string cacheKey = stateData.Data;
        (string userId, string clientIp) = await _cacheService.GetAsync<Tuple<string, string>>(cacheKey, cancellationToken) ??
                                           throw new UnauthorizedException(_t["Authentication Failed."]);
        await _cacheService.RemoveAsync(cacheKey, cancellationToken);

        if (clientIp.IsNullOrEmpty())
        {
            throw new UnauthorizedException(_t["Authentication Failed."]);
        }

        if (clientIp != ipAddress)
        {
            throw new UnauthorizedException(_t["Authentication Failed."]);
        }

        if (string.IsNullOrWhiteSpace(userId))
        {
            throw new UnauthorizedException(_t["Authentication Failed."]);
        }

        if(tenantId != _currentTenant.Id)
        {
            throw new UnauthorizedException(_t["Authentication Failed."]);
        }

        if (expireAt < DateTimeOffset.UtcNow)
        {
            throw new UnauthorizedException(_t["Authentication Failed."]);
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            throw new UnauthorizedException(_t["Authentication Failed."]);
        }

        string? oldUserRefreshToken = _httpContextAccessor.HttpContext?.Request.Cookies[_refreshTokenCookieName];
        if (!string.IsNullOrEmpty(oldUserRefreshToken))
        {
            await RevokeRefreshToken(oldUserRefreshToken);
        }

        return await GenerateTokensAndUpdateUser(user, ipAddress);
    }

    public async Task<TokenResponse> RefreshTokenAsync(string ipAddress)
    {
        if (_currentTenant == null || string.IsNullOrWhiteSpace(_currentTenant.Id))
        {
            throw new UnauthorizedException(_t["Authentication Failed."]);
        }

        string? refreshToken = _httpContextAccessor.HttpContext?.Request.Cookies[_refreshTokenCookieName];

        if (string.IsNullOrEmpty(refreshToken))
        {
            throw new UnauthorizedException(_t["Authentication Failed."]);
        }

        var user = await _userManager
            .Users.Include(x => x.RefreshTokens)
            .SingleOrDefaultAsync(x => x.RefreshTokens.Any(y => y.Token == refreshToken));

        if (user == null)
            throw new UnauthorizedException(_t["Authentication Failed."]);

        await RevokeRefreshToken(refreshToken);

        return await GenerateTokensAndUpdateUser(user, ipAddress);
    }

    public async Task<string> RevokeCurrentUserRefreshTokenAsync()
    {
        string userId = _currentUser.GetUserId().ToString();

        var user = await _userManager.Users
            .Include(x => x.RefreshTokens)
            .SingleOrDefaultAsync(x => x.Id == userId);
        if (user == null)
        {
            throw new NotFoundException(_t["User not found."]);
        }

        // Delete the refresh token cookie
        if (_httpContextAccessor.HttpContext.Request.Cookies.ContainsKey(_refreshTokenCookieName))
        {
            var refreshToken = user.RefreshTokens.Single(x => x.Token == _httpContextAccessor.HttpContext?.Request.Cookies[_refreshTokenCookieName]);
            if (refreshToken.IsActive)
            {
                refreshToken.Revoked = DateTime.UtcNow;
                await _userManager.UpdateAsync(user);
            }

            _httpContextAccessor.HttpContext.Response.Cookies.Delete(_refreshTokenCookieName);
        }

        return _httpContextAccessor.HttpContext.Request.GetOrigin();
    }

    private async Task<TokenResponse> GenerateTokensAndUpdateUser(ApplicationUser user, string ipAddress)
    {
        string token = GenerateJwt(user, ipAddress);

        await SetRefreshToken(user);

        return await Task.FromResult(new TokenResponse(token));
    }

    private string GenerateJwt(ApplicationUser user, string ipAddress) =>
        GenerateEncryptedToken(GetSigningCredentials(), GetClaims(user, ipAddress));

    private IEnumerable<Claim> GetClaims(ApplicationUser user, string ipAddress) =>
        new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email!),
            new(ApplicationClaims.Fullname, $"{user.FirstName} {user.LastName}"),
            new(ClaimTypes.Name, user.FirstName ?? string.Empty),
            new(ClaimTypes.Surname, user.LastName ?? string.Empty),
            new(ApplicationClaims.IpAddress, ipAddress),
            new(ApplicationClaims.Tenant, _currentTenant!.Id),
            new(ApplicationClaims.ImageUrl, user.ImageUrl ?? string.Empty),
            new(ClaimTypes.MobilePhone, user.PhoneNumber ?? string.Empty)
        };

    private string GenerateEncryptedToken(SigningCredentials signingCredentials, IEnumerable<Claim> claims)
    {
        var token = new JwtSecurityToken(
           claims: claims,
           expires: DateTime.UtcNow.AddMinutes(_jwtSettings.TokenExpirationInMinutes),
           signingCredentials: signingCredentials);
        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }

    private SigningCredentials GetSigningCredentials()
    {
        byte[] secret = Encoding.UTF8.GetBytes(_jwtSettings.Key);
        return new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);
    }

    private async Task SetRefreshToken(ApplicationUser user)
    {
        string refreshToken = TokenGenerator.GenerateToken();
        user.RefreshTokens.Add(new ApplicationUserRefreshToken
        {
            Token = refreshToken,
            Expires = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationInDays),
        });
        await _userManager.UpdateAsync(user);

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationInDays),
        };

        _httpContextAccessor.HttpContext.Response.Cookies.Append(_refreshTokenCookieName, refreshToken, cookieOptions);
    }

    private async Task RevokeRefreshToken(string refreshToken)
    {
        if (string.IsNullOrEmpty(refreshToken))
        {
            return;
        }

        var user = await _userManager
            .Users.Include(x => x.RefreshTokens)
            .SingleOrDefaultAsync(x => x.RefreshTokens.Any(y => y.Token == refreshToken));

        if (user == null)
        {
            return;
        }

        var oldRefreshToken = user.RefreshTokens.Single(x => x.Token == refreshToken);
        if (!oldRefreshToken.IsActive)
        {
            return;
        }

        oldRefreshToken.Revoked = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);
    }
}