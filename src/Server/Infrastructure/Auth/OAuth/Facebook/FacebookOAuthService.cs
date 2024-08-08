using Facebook;

namespace MultiMart.Infrastructure.Auth.OAuth.Facebook;

public class FacebookOAuthService
{
    private readonly FacebookSettings _facebookSettings;
    private readonly string _redirectUri;

    public FacebookOAuthService(FacebookSettings facebookSettings, string redirectUri)
    {
        _facebookSettings = facebookSettings;
        _redirectUri = redirectUri;
    }

    public string GetLoginLinkUrl(string? state = null)
    {
        var fb = new FacebookClient();

        var loginUrl = fb.GetLoginUrl(new
        {
            client_id = _facebookSettings.AppId,
            client_secret = _facebookSettings.AppSecret,
            redirect_uri = _redirectUri,
            response_type = "code",
            scope = "email",
            state,
        });

        return loginUrl.AbsoluteUri;
    }

    public async Task<Userinfo> GetUserFromCode(string code)
    {
        var fb = new FacebookClient();

        dynamic result = fb.Post("oauth/access_token", new
        {
            client_id = _facebookSettings.AppId,
            client_secret = _facebookSettings.AppSecret,
            redirect_uri = _redirectUri,
            code
        });

        var accessToken = result.access_token;

        fb.AccessToken = accessToken;

        dynamic me = await fb.GetTaskAsync("me", new { fields = "id,name,email,picture.width(100).height(100)" });

        var userinfo = new Userinfo
        {
            Id = me.id,
            Name = me.name,
            Email = string.IsNullOrEmpty(me.email) ? $"{me.id + "@facebook.com"}" : me.email,
            Picture = me.picture.data.url
        };

        return userinfo;
    }
}