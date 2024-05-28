namespace MultiMart.Infrastructure.Security.SecurityHeaders;

public class SecurityHeaderSettings
{
    public bool Enable { get; set; }
    public MultiMart.Infrastructure.Security.SecurityHeaders.SecurityHeaders Headers { get; set; } = default!;
}