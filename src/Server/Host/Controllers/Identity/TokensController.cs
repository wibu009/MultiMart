using BookStack.Application.Identity.Tokens;
using BookStack.Infrastructure.Common.Extensions;

namespace BookStack.Host.Controllers.Identity;

public sealed class TokensController : VersionNeutralApiController
{
    private readonly ITokenService _tokenService;

    public TokensController(ITokenService tokenService) => _tokenService = tokenService;

    [HttpPost]
    [AllowAnonymous]
    [TenantIdHeader]
    [OpenApiOperation("Request an access token using credentials.", "")]
    public Task<TokenResponse> GetTokenAsync(TokenRequest request, CancellationToken cancellationToken)
    {
        return _tokenService.GetTokenAsync(request, Request.GetIpAddress(), cancellationToken);
    }

    [HttpGet("refresh")]
    [AllowAnonymous]
    [TenantIdHeader]
    [OpenApiOperation("Request an access token using a refresh token.", "")]
    [ApiConventionMethod(typeof(ApplicationApiConventions), nameof(ApplicationApiConventions.Search))]
    public Task<TokenResponse> RefreshAsync([FromQuery] string? state)
    {
        return state == null
            ? _tokenService.RefreshTokenAsync(Request.GetIpAddress())
            : _tokenService.RefreshTokenAsync(Request.GetIpAddress(), state);
    }
}