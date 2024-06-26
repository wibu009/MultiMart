using MultiMart.Application.Identity.Tokens;
using MultiMart.Infrastructure.Common.Extensions;
using MultiMart.Infrastructure.OpenApi;

namespace MultiMart.Host.Controllers.Identity;

public sealed class TokensController : VersionNeutralApiController
{
    private readonly ITokenService _tokenService;

    public TokensController(ITokenService tokenService) => _tokenService = tokenService;

    [HttpPost]
    [AllowAnonymous]
    [TenantIdHeader]
    [SwaggerOperation("Request an access token using credentials.", "")]
    public async Task<TokenResponse> GetTokenAsync(
        [FromQuery] string? token,
        [FromBody] TokenRequest request,
        CancellationToken cancellationToken)
    {
        return token is not null
            ? await _tokenService.GetTokenAsync(token, Request.GetIpAddress(), cancellationToken)
            : await _tokenService.GetTokenAsync(request, Request.GetIpAddress(), cancellationToken);
    }

    [HttpGet("refresh")]
    [AllowAnonymous]
    [TenantIdHeader]
    [SwaggerOperation("Request an access token using a refresh token.", "")]
    public async Task<TokenResponse> RefreshAsync()
    {
        return await _tokenService.RefreshTokenAsync(Request.GetIpAddress());
    }

    [HttpPost("revoke")]
    [SwaggerOperation("Revoke current user's refresh token.", "")]
    public async Task<IActionResult> RevokeAsync()
    {
        return HandleRedirect(await _tokenService.RevokeCurrentUserRefreshTokenAsync());
    }
}