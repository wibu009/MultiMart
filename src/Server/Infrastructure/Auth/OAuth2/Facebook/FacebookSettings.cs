namespace BookStack.Infrastructure.Auth.OAuth2.Facebook;

public class FacebookSettings
{
    public string AppId { get; set; } = default!;
    public string AppSecret { get; set; } = default!;
    public string CallBackPath { get; set; } = default!;
}