using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using MultiMart.Application.Common.Caching;
using MultiMart.Application.Common.Events;
using MultiMart.Domain.Common.Enums;
using MultiMart.Domain.Identity;
using MultiMart.Infrastructure.Auth.OAuth2.Facebook;
using MultiMart.Infrastructure.Auth.OAuth2.Google;
using MultiMart.Infrastructure.Common;
using MultiMart.Infrastructure.Common.Extensions;
using MultiMart.Infrastructure.Identity.Token;
using MultiMart.Infrastructure.Identity.User;
using MultiMart.Infrastructure.Multitenancy;
using MultiMart.Shared.Authorization;
using SendGrid.Helpers.Errors.Model;

namespace MultiMart.Infrastructure.Auth.OAuth2;

public class OAuth2Service
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly GoogleOAuth2Service _googleOAuth2Service;
    private readonly FacebookOAuth2Service _facebookOAuth2Service;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IStringLocalizer _t;
    private readonly ApplicationTenantInfo _currentTenant;
    private readonly ICacheService _cacheService;
    private readonly IEventPublisher _events;

    public OAuth2Service(
        IHttpContextAccessor httpContextAccessor,
        IOptions<GoogleSettings> googleSettings,
        IOptions<FacebookSettings> facebookSettings,
        UserManager<ApplicationUser> userManager,
        IStringLocalizer<OAuth2Service> t,
        ApplicationTenantInfo currentTenant,
        ICacheService cacheService,
        IEventPublisher events)
    {
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
        _t = t;
        _currentTenant = currentTenant;
        _cacheService = cacheService;
        _events = events;
        _googleOAuth2Service = new GoogleOAuth2Service(
            googleSettings.Value,
            $"{httpContextAccessor.HttpContext!.Request.Scheme}://{httpContextAccessor.HttpContext!.Request.Host}{googleSettings.Value.CallBackPath}");
        _facebookOAuth2Service =
            new FacebookOAuth2Service(
                facebookSettings.Value,
                $"{httpContextAccessor.HttpContext!.Request.Scheme}://{httpContextAccessor.HttpContext!.Request.Host}{facebookSettings.Value.CallBackPath}");
    }

    public async Task<string> ExternalAuthAsync(string provider)
    {
        string cacheKey = TokenGenerator.GenerateToken();
        await _cacheService.SetAsync(
            cacheKey,
            Tuple.Create(
                _httpContextAccessor.HttpContext.Request.GetUri(),
                _httpContextAccessor.HttpContext.GetIpAddress()),
            TimeSpan.FromMinutes(2));

        var stateData = new StateData<string>(
            _currentTenant.Id,
            cacheKey,
            DateTimeOffset.UtcNow.AddMinutes(2));
        string state = stateData.ToBase64String();

        return await Task.FromResult(provider.ToLower() switch
        {
            "facebook" => _facebookOAuth2Service.GetLoginLinkUrl(state),
            "google" => _googleOAuth2Service.GetLoginLinkUrl(state),
            _ => throw new BadRequestException(_t["Invalid provider."])
        });
    }

    public async Task<string> ExternalAuthCallbackAsync(string provider, string code, string? error = "", string? state = "")
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

        var stateData = state == null ? throw new BadRequestException(_t["Invalid state."]) : state.FromBase64String<StateData<string>>();
        string tenantId = stateData.TenantId;
        (string clientUrl, string clientIp) = await _cacheService.GetAsync<Tuple<string, string>>(stateData.Data) ?? throw new BadRequestException(_t["Invalid state."]);
        await _cacheService.RemoveAsync(stateData.Data);

        dynamic? userExist = await _userManager.FindByEmailAsync(user.Email);
        string token;
        string tokenCacheKey = TokenGenerator.GenerateToken();
        if (userExist != null)
        {
            await _cacheService.SetAsync(
                tokenCacheKey,
                Tuple.Create(userExist.Id, clientIp),
                TimeSpan.FromMinutes(2));

            token = new StateData<string>(
                    tenantId,
                    tokenCacheKey,
                    DateTimeOffset.UtcNow.AddMinutes(2))
                .ToBase64String();
            return QueryHelpers.AddQueryString(clientUrl, QueryStringKeys.Token, token);
        }

        string[] nameParts = user.Name.Split(" ");
        var appUser = new ApplicationUser
        {
            Id = DefaultIdType.NewGuid().ToString(),
            UserName = user.Email,
            Email = user.Email,
            FirstName = nameParts[0],
            Gender = Gender.NotKnown,
            DateOfBirth = null,
            LastName = nameParts.Length > 1 ? string.Join(" ", nameParts.Skip(1)) : string.Empty,
            Avatar = user.Picture,
            EmailConfirmed = false,
        };

        await _userManager.CreateAsync(appUser);
        await _userManager.AddToRoleAsync(appUser, ApplicationRoles.Basic);

        await _cacheService.SetAsync(
            tokenCacheKey,
            Tuple.Create(appUser.Id, clientIp),
            TimeSpan.FromMinutes(2));
        token = new StateData<string>(
                tenantId,
                tokenCacheKey,
                DateTimeOffset.UtcNow.AddMinutes(2))
            .ToBase64String();

        await _events.PublishAsync(new ApplicationUserCreatedEvent(appUser.Id));

        return QueryHelpers.AddQueryString(clientUrl, QueryStringKeys.Token, token);
    }
}