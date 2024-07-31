namespace MultiMart.Application.Identity.Tokens.Get;

public class GetTokenRequestHandler : IRequestHandler<GetTokenRequest, TokenResponse>
{
private readonly ITokenService _tokenService;

public GetTokenRequestHandler(ITokenService tokenService) => _tokenService = tokenService;

public Task<TokenResponse> Handle(GetTokenRequest request, CancellationToken cancellationToken)
    => _tokenService.GetTokenAsync(request, cancellationToken);
}