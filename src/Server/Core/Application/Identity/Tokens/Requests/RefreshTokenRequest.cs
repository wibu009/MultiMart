using MultiMart.Application.Identity.Tokens.Interfaces;
using MultiMart.Application.Identity.Tokens.Models;

namespace MultiMart.Application.Identity.Tokens.Requests;

public class RefreshTokenRequest : IRequest<TokenResponse>
{
    public string IpAddress { get; set; } = default!;
}

public class RefreshTokenRequestHandler : IRequestHandler<RefreshTokenRequest, TokenResponse>
{
    private readonly ITokenService _tokenService;

    public RefreshTokenRequestHandler(ITokenService tokenService) => _tokenService = tokenService;

    public Task<TokenResponse> Handle(RefreshTokenRequest request, CancellationToken cancellationToken)
        => _tokenService.RefreshTokenAsync(request.IpAddress);
}