using MultiMart.Infrastructure.Auth.OAuth;
using MultiMart.Infrastructure.OpenApi;

namespace MultiMart.Host.Controllers.OAuth;

public class OAuthController : VersionNeutralApiController
{
    private readonly OAuthService _oauthService;

    public OAuthController(OAuthService oauthService) => _oauthService = oauthService;

    [HttpGet("{provider}")]
    [AllowAnonymous]
    [TenantIdHeader]
    [SwaggerOperation("External authentication", "Supports Google and Facebook.")]
    public async Task<IActionResult> ExternalLogin([FromRoute] string provider)
        => RedirectIfNotSwagger(await _oauthService.ExternalAuthAsync(provider));

    [HttpGet("callback/{provider}")]
    [AllowAnonymous]
    [SwaggerOperation("External authentication callback", "Supports Google and Facebook.")]
    public async Task<IActionResult> ExternalLoginCallback(
        [FromRoute] string provider,
        [FromQuery] string code,
        [FromQuery] string? error = "",
        [FromQuery] string? state = "")
        => RedirectIfNotSwagger(await _oauthService.ExternalAuthCallbackAsync(provider, code, error, state));
}