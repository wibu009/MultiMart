using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BookStack.Application.Common.Exceptions;
using BookStack.Application.Identity.Tokens;
using BookStack.Infrastructure.Auth;
using BookStack.Infrastructure.Auth.Jwt;
using BookStack.Infrastructure.Auth.OAuth2;
using BookStack.Infrastructure.Common.Extensions;
using BookStack.Infrastructure.Identity.User;
using BookStack.Infrastructure.Multitenancy;
using BookStack.Infrastructure.Security.Encrypt;
using BookStack.Shared.Authorization;
using BookStack.Shared.Multitenancy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BookStack.Infrastructure.Identity.Token;

internal class TokenService : ITokenService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IStringLocalizer _t;
    private readonly SecuritySettings _securitySettings;
    private readonly JwtSettings _jwtSettings;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ApplicationTenantInfo? _currentTenant;
    private readonly EncryptionSettings _encryptionSettings;

    public TokenService(
        UserManager<ApplicationUser> userManager,
        IOptions<JwtSettings> jwtSettings,
        IStringLocalizer<TokenService> t,
        ApplicationTenantInfo? currentTenant,
        IOptions<SecuritySettings> securitySettings,
        IHttpContextAccessor httpContextAccessor,
        IOptions<EncryptionSettings> encryptionSettings)
    {
        _userManager = userManager;
        _t = t;
        _jwtSettings = jwtSettings.Value;
        _currentTenant = currentTenant;
        _httpContextAccessor = httpContextAccessor;
        _securitySettings = securitySettings.Value;
        _encryptionSettings = encryptionSettings.Value;
    }

    public async Task<TokenResponse> GetTokenAsync(TokenRequest request, string ipAddress,
        CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.UserNameOrEmail?.Trim().Normalize())
                   ?? await _userManager.FindByNameAsync(request.UserNameOrEmail?.Trim().Normalize());

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

        return await GenerateTokensAndUpdateUser(user, ipAddress);
    }

    public async Task<TokenResponse> RefreshTokenAsync(string ipAddress)
    {
        if (_currentTenant == null || string.IsNullOrWhiteSpace(_currentTenant.Id))
        {
            throw new UnauthorizedException(_t["Authentication Failed."]);
        }

        // Decrypt the cookie name before using it
        string cookieName = $"refreshToken-{_currentTenant.Id}";
        string encryptedCookieName = EncryptCookieName(cookieName);
        string? refreshToken = _httpContextAccessor.HttpContext?.Request.Cookies[encryptedCookieName];

        if (string.IsNullOrEmpty(refreshToken))
        {
            throw new UnauthorizedException(_t["Authentication Failed."]);
        }

        var user = await _userManager
            .Users.Include(x => x.RefreshTokens)
            .SingleOrDefaultAsync(x => x.RefreshTokens.Any(y => y.Token == refreshToken));

        if (user == null)
            throw new UnauthorizedException(_t["Authentication Failed."]);

        var oldRefreshToken = user.RefreshTokens.Single(x => x.Token == refreshToken);
        if (!oldRefreshToken.IsActive)
            throw new UnauthorizedException(_t["Authentication Failed."]);
        oldRefreshToken.Revoked = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);

        return await GenerateTokensAndUpdateUser(user, ipAddress);
    }

    public async Task<TokenResponse> RefreshTokenAsync(string ipAddress, string? state)
    {
        if (_currentTenant == null || string.IsNullOrWhiteSpace(_currentTenant.Id))
        {
            throw new UnauthorizedException(_t["Authentication Failed."]);
        }

        var stateData = state?.Decrypt<StateData<CallbackStateData>>(_encryptionSettings.Key, _encryptionSettings.IV);
        string tenantId = stateData.TenantId;
        var expireAt = stateData.ExpireAt;
        string clientIp = stateData.Data.ClientIp;
        string userId = stateData.Data.UserId;

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

        return await GenerateTokensAndUpdateUser(user, ipAddress);
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
        string cookieName = $"refreshToken-{_currentTenant.Id}";
        string encryptedCookieName = EncryptCookieName(cookieName);
        _httpContextAccessor.HttpContext?.Response.Cookies.Append(encryptedCookieName, refreshToken, cookieOptions);
    }

    private string EncryptCookieName(string cookieName)
    {
        string key = _jwtSettings.RefreshTokenSecretKey;

        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key));
        byte[] cookieNameBytes = Encoding.UTF8.GetBytes(cookieName);
        byte[] hashedCookieName = hmac.ComputeHash(cookieNameBytes);

        return Convert.ToBase64String(hashedCookieName).Replace('+', '-').Replace('/', '_').TrimEnd('=');
    }

    private string DecryptCookieName(string encryptedCookieName)
    {
        string key = _jwtSettings.RefreshTokenSecretKey;

        // Replace '-' with '+', '_' with '/', and add the correct number of padding characters
        encryptedCookieName = encryptedCookieName.Replace('-', '+').Replace('_', '/') + new string('=', (4 - (encryptedCookieName.Length % 4)) % 4);

        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key));
        byte[] encryptedCookieNameBytes = Convert.FromBase64String(encryptedCookieName);
        byte[] decryptedCookieNameBytes = hmac.ComputeHash(encryptedCookieNameBytes);

        return Encoding.UTF8.GetString(decryptedCookieNameBytes);
    }
}