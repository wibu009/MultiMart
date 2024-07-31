namespace MultiMart.Application.Identity.Tokens.Refresh;

public class RefreshTokenRequest : IRequest<TokenResponse>
{
    public string IpAddress { get; set; } = default!;
}