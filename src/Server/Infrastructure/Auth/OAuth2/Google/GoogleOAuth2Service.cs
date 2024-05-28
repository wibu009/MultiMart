using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Oauth2.v2;
using Google.Apis.Oauth2.v2.Data;
using Google.Apis.Services;

namespace MultiMart.Infrastructure.Auth.OAuth2.Google;

public class GoogleOAuth2Service
{
    private readonly GoogleSettings _googleSettings;
    private readonly string _redirectUri;

    public GoogleOAuth2Service(GoogleSettings googleSettings, string redirectUri)
    {
        _googleSettings = googleSettings;
        _redirectUri = redirectUri;
        //$"{httpContextAccessor.HttpContext!.Request.Scheme}://{httpContextAccessor.HttpContext!.Request.Host}{_googleSettings.CallBackPath}";
    }

    public string GetLoginLinkUrl(string? state = null)
    {
        var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
        {
            ClientSecrets = new ClientSecrets
            {
                ClientId = _googleSettings.ClientId,
                ClientSecret = _googleSettings.ClientSecret
            },
            Scopes = new[] { "email", "profile" },
        });

        var url = flow.CreateAuthorizationCodeRequest(_redirectUri);

        if (!string.IsNullOrEmpty(state))
        {
            url.State = state;
        }

        return url.Build().AbsoluteUri;
    }

    public async Task<Userinfo> GetUserFromCode(string code)
    {
        var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
        {
            ClientSecrets = new ClientSecrets
            {
                ClientId = _googleSettings.ClientId,
                ClientSecret = _googleSettings.ClientSecret
            },
            Scopes = new[] { "email", "profile" },
        });

        var token = await flow.ExchangeCodeForTokenAsync("", code, _redirectUri, CancellationToken.None);

        var credentials = new UserCredential(flow, "", token);

        var service = new Oauth2Service(new BaseClientService.Initializer
        {
            HttpClientInitializer = credentials,
            ApplicationName = "RadzenBook"
        });

        return await service.Userinfo.Get().ExecuteAsync();
    }
}