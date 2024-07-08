namespace MultiMart.Infrastructure.Mailing.SendGrid;

public class SendGridMailSettings
{
    public string ApiKey { get; set; } = default!;
    public string From { get; set; } = default!;
    public string DisplayName { get; set; } = default!;
}