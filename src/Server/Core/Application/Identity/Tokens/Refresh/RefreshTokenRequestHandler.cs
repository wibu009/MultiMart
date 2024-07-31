namespace MultiMart.Application.Identity.Tokens.Refresh;

public class RefreshTokenRequestHandler : IRequestHandler<RefreshTokenRequest, TokenResponse>
{
    private readonly ITokenService _tokenService;

    public RefreshTokenRequestHandler(ITokenService tokenService) => _tokenService = tokenService;

    public Task<TokenResponse> Handle(RefreshTokenRequest request, CancellationToken cancellationToken)
        => _tokenService.RefreshTokenAsync(request.IpAddress);
}