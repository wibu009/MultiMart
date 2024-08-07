using System.Text.Json.Serialization;

namespace MultiMart.Application.Identity.Tokens.Get;

public class GetTokenRequest : IRequest<TokenResponse>
{
    [JsonIgnore]
    public string? Token { get; set; } = default!;
    public string? UserNameOrEmail { get; set; } = default!;
    public string? Password { get; set; } = default!;
    [JsonIgnore]
    public string? IpAddress { get; set; } = default!;
}

public class TokenRequestHandler : IRequestHandler<GetTokenRequest, TokenResponse>
{
    private readonly ITokenService _tokenService;

    public TokenRequestHandler(ITokenService tokenService) => _tokenService = tokenService;

    public Task<TokenResponse> Handle(GetTokenRequest request, CancellationToken cancellationToken)
        => _tokenService.GetTokenAsync(request, cancellationToken);
}