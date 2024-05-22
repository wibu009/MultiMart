namespace BookStack.Infrastructure.Mailing.SendGrid;

public class SendGridMailSettings
{
    public string Key { get; set; } = default!;
    public string From { get; set; } = default!;
    public string DisplayName { get; set; } = default!;
}