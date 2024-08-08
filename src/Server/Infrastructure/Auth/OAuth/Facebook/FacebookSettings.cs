namespace MultiMart.Infrastructure.Auth.OAuth.Facebook;

public class FacebookSettings
{
    public string AppId { get; set; } = default!;
    public string AppSecret { get; set; } = default!;
    public string CallBackPath { get; set; } = default!;
}