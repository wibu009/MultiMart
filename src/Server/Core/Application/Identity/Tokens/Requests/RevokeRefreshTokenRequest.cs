using MultiMart.Application.Identity.Tokens.Interfaces;

namespace MultiMart.Application.Identity.Tokens.Requests;

public class RevokeRefreshTokenRequest : IRequest<string>
{
}

public class RevokeRefreshTokenRequestHandler : IRequestHandler<RevokeRefreshTokenRequest, string>
{
    private readonly ITokenService _tokenService;

    public RevokeRefreshTokenRequestHandler(ITokenService tokenService) => _tokenService = tokenService;

    public Task<string> Handle(RevokeRefreshTokenRequest request, CancellationToken cancellationToken)
        => _tokenService.RevokeCurrentUserRefreshTokenAsync();
}