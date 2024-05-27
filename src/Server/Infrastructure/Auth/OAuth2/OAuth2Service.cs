using BookStack.Application.Common.Caching;
using BookStack.Infrastructure.Auth.OAuth2.Facebook;
using BookStack.Infrastructure.Auth.OAuth2.Google;
using BookStack.Infrastructure.Common.Extensions;
using BookStack.Infrastructure.Identity.Token;
using BookStack.Infrastructure.Identity.User;
using BookStack.Infrastructure.Multitenancy;
using BookStack.Infrastructure.Security.Encrypt;
using BookStack.Shared.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using SendGrid.Helpers.Errors.Model;

namespace BookStack.Infrastructure.Auth.OAuth2;

public class OAuth2Service
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly GoogleOAuth2Service _googleOAuth2Service;
    private readonly FacebookOAuth2Service _facebookOAuth2Service;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IStringLocalizer _t;
    private readonly ApplicationTenantInfo _currentTenant;
    private readonly EncryptionSettings _encryptionSettings;
    private readonly ICacheService _cacheService;

    public OAuth2Service(
        IHttpContextAccessor httpContextAccessor,
        IOptions<GoogleSettings> googleSettings,
        IOptions<FacebookSettings> facebookSettings,
        UserManager<ApplicationUser> userManager,
        IStringLocalizer<OAuth2Service> t,
        ApplicationTenantInfo currentTenant,
        IOptions<EncryptionSettings> encryptionSettings,
        ICacheService cacheService)
    {
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
        _t = t;
        _currentTenant = currentTenant;
        _cacheService = cacheService;
        _googleOAuth2Service = new GoogleOAuth2Service(
            googleSettings.Value,
            $"{httpContextAccessor.HttpContext!.Request.Scheme}://{httpContextAccessor.HttpContext!.Request.Host}{googleSettings.Value.CallBackPath}");
        _facebookOAuth2Service =
            new FacebookOAuth2Service(
                facebookSettings.Value,
                $"{httpContextAccessor.HttpContext!.Request.Scheme}://{httpContextAccessor.HttpContext!.Request.Host}{facebookSettings.Value.CallBackPath}");
        _encryptionSettings = encryptionSettings.Value;
    }

    public async Task<string> ExternalAuthAsync(string provider)
    {
        string cacheKey = TokenGenerator.GenerateToken();
        await _cacheService.SetAsync(
            cacheKey,
            Tuple.Create(
                _httpContextAccessor.HttpContext.Request.GetUrlFromRequest(),
                _httpContextAccessor.HttpContext.GetIpAddress()),
            TimeSpan.FromMinutes(2));

        var stateData = new StateData<string>(
            _currentTenant.Id,
            cacheKey,
            DateTimeOffset.UtcNow.AddMinutes(2));
        string state = stateData.Encrypt(_encryptionSettings.Key, _encryptionSettings.IV);

        return await Task.FromResult(provider.ToLower() switch
        {
            "facebook" => _facebookOAuth2Service.GetLoginLinkUrl(state),
            "google" => _googleOAuth2Service.GetLoginLinkUrl(state),
            _ => throw new BadRequestException(_t["Invalid provider."])
        });
    }

    public async Task<string> ExternalAuthCallbackAsync(string provider, string code, string? error = "",
        string? state = "")
    {
        dynamic user = provider.ToLower() switch
        {
            "google" => await _googleOAuth2Service.GetUserFromCode(code),
            "facebook" => await _facebookOAuth2Service.GetUserFromCode(code),
            _ => throw new BadRequestException(_t["Invalid provider."])
        };

        if (user == null)
        {
            throw new BadRequestException(_t["External auth has failed."]);
        }

        var stateData = state == null ? throw new BadRequestException(_t["Invalid state."]) : state.Decrypt<StateData<string>>(_encryptionSettings.Key, _encryptionSettings.IV);
        string tenantId = stateData.TenantId;
        (string clientUrl, string clientIp) = await _cacheService.GetAsync<Tuple<string, string>>(stateData.Data) ?? throw new BadRequestException(_t["Invalid state."]);
        await _cacheService.RemoveAsync(stateData.Data);

        dynamic? userExist = await _userManager.FindByEmailAsync(user.Email);
        string signInToken;
        string signInTokenCacheKey = TokenGenerator.GenerateToken();
        if (userExist != null)
        {
            await _cacheService.SetAsync(
                signInTokenCacheKey,
                Tuple.Create(userExist.Id, clientIp),
                TimeSpan.FromMinutes(2));

            signInToken = new StateData<string>(
                tenantId,
                signInTokenCacheKey,
                DateTimeOffset.UtcNow.AddMinutes(2))
                .Encrypt(_encryptionSettings.Key, _encryptionSettings.IV);
            return clientUrl.AddQueryParam("sign_in_token", signInToken);
        }

        string[] nameParts = user.Name.Split(" ");
        var appUser = new ApplicationUser
        {
            Id = Guid.NewGuid().ToString(),
            UserName = user.Email,
            Email = user.Email,
            FirstName = nameParts[0],
            LastName = nameParts.Length > 1 ? string.Join(" ", nameParts.Skip(1)) : string.Empty,
            ImageUrl = user.Picture,
            EmailConfirmed = false,
        };

        await _userManager.CreateAsync(appUser);
        await _userManager.AddToRoleAsync(appUser, ApplicationRoles.Basic);

        await _cacheService.SetAsync(
            signInTokenCacheKey,
            Tuple.Create(appUser.Id, clientIp),
            TimeSpan.FromMinutes(2));
        signInToken = new StateData<string>(
            tenantId,
            signInTokenCacheKey,
            DateTimeOffset.UtcNow.AddMinutes(2))
            .Encrypt(_encryptionSettings.Key, _encryptionSettings.IV);

        return clientUrl.AddQueryParam("sign_token", signInToken);
    }
}