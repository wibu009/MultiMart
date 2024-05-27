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
    public async Task<TokenResponse> GetTokenAsync(
        [FromQuery] string? signInToken,
        [FromBody] TokenRequest request,
        CancellationToken cancellationToken)
    {
        return signInToken is not null
            ? await _tokenService.GetTokenAsync(signInToken, Request.GetIpAddress(), cancellationToken)
            : await _tokenService.GetTokenAsync(request, Request.GetIpAddress(), cancellationToken);
    }

    [HttpGet("refresh")]
    [AllowAnonymous]
    [TenantIdHeader]
    [OpenApiOperation("Request an access token using a refresh token.", "")]
    public async Task<TokenResponse> RefreshAsync()
    {
        return await _tokenService.RefreshTokenAsync(Request.GetIpAddress());
    }

    [HttpPost("revoke")]
    [OpenApiOperation("Revoke a refresh token.", "")]
    public async Task<IActionResult> RevokeAsync()
    {
        return HandleRedirect(await _tokenService.RevokeRefreshTokenAsync());
    }
}