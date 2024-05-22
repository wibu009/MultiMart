using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BookStack.Application.Common.Caching;
using BookStack.Application.Common.Exceptions;
using BookStack.Application.Identity.Tokens;
using BookStack.Infrastructure.Auth;
using BookStack.Infrastructure.Auth.Jwt;
using BookStack.Infrastructure.Common.Extensions;
using BookStack.Infrastructure.Identity.User;
using BookStack.Infrastructure.Multitenancy;
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
    private readonly ICacheService _cacheService;
    private readonly ApplicationTenantInfo? _currentTenant;

    public TokenService(
        UserManager<ApplicationUser> userManager,
        IOptions<JwtSettings> jwtSettings,
        IStringLocalizer<TokenService> t,
        ApplicationTenantInfo? currentTenant,
        IOptions<SecuritySettings> securitySettings,
        IHttpContextAccessor httpContextAccessor,
        ICacheService cacheService)
    {
        _userManager = userManager;
        _t = t;
        _jwtSettings = jwtSettings.Value;
        _currentTenant = currentTenant;
        _httpContextAccessor = httpContextAccessor;
        _cacheService = cacheService;
        _securitySettings = securitySettings.Value;
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

        string? refreshToken = _httpContextAccessor.HttpContext?.Request.Cookies["refreshToken"];

        if (string.IsNullOrEmpty(refreshToken))
        {
            throw new UnauthorizedException(_t["Invalid Refresh Token."]);
        }

        var user = await _userManager
            .Users.Include(x => x.RefreshTokens)
            .SingleOrDefaultAsync(x => x.RefreshTokens.Any(y => y.Token == refreshToken));

        if (user == null)
            throw new UnauthorizedException(_t["Invalid Refresh Token."]);

        var oldRefreshToken = user.RefreshTokens.Single(x => x.Token == refreshToken);
        if (!oldRefreshToken.IsActive)
            throw new UnauthorizedException(_t["Invalid Refresh Token."]);
        oldRefreshToken.Revoked = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);

        return await GenerateTokensAndUpdateUser(user, ipAddress);
    }

    public async Task<TokenResponse> RefreshTokenAsync(string ipAddress, string? signInCode)
    {
        if (_currentTenant == null || string.IsNullOrWhiteSpace(_currentTenant.Id))
        {
            throw new UnauthorizedException(_t["Authentication Failed."]);
        }
        (var userId, string clientIp) = _cacheService.Get<Tuple<Guid, string>>(signInCode ?? throw new ArgumentNullException(nameof(signInCode))) ?? throw new InvalidOperationException();

        if (userId == Guid.Empty || clientIp.IsNullOrEmpty())
        {
            throw new UnauthorizedException(_t["Invalid Sign In Code."]);
        }

        await _cacheService.RemoveAsync(signInCode!);

        if (clientIp != _httpContextAccessor.HttpContext!.GetIpAddress())
        {
            throw new UnauthorizedException(_t["Invalid Sign In Code."]);
        }

        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            throw new UnauthorizedException(_t["Invalid Sign In Code."]);
        }
        return await GenerateTokensAndUpdateUser(user, ipAddress);
    }

    private async Task<TokenResponse> GenerateTokensAndUpdateUser(ApplicationUser user, string ipAddress)
    {
        string token = GenerateJwt(user, ipAddress);

        await SetRefreshToken(user, ipAddress);

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

    private static string GenerateRefreshToken()
    {
        byte[] randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private string GenerateEncryptedToken(SigningCredentials signingCredentials, IEnumerable<Claim> claims)
    {
        var token = new JwtSecurityToken(
           claims: claims,
           expires: DateTime.UtcNow.AddMinutes(_jwtSettings.TokenExpirationInMinutes),
           signingCredentials: signingCredentials);
        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }

    private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key)),
            ValidateIssuer = false,
            ValidateAudience = false,
            RoleClaimType = ClaimTypes.Role,
            ClockSkew = TimeSpan.Zero,
            ValidateLifetime = false
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(
                SecurityAlgorithms.HmacSha256,
                StringComparison.OrdinalIgnoreCase))
        {
            throw new UnauthorizedException(_t["Invalid Token."]);
        }

        return principal;
    }

    private SigningCredentials GetSigningCredentials()
    {
        byte[] secret = Encoding.UTF8.GetBytes(_jwtSettings.Key);
        return new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);
    }

    private async Task SetRefreshToken(ApplicationUser user, string ipAddress)
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
        _httpContextAccessor.HttpContext?.Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
    }
}