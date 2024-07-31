using MultiMart.Infrastructure.Auth.OAuth2;
using MultiMart.Infrastructure.OpenApi;

namespace MultiMart.Host.Controllers.OAuth2;

public class OAuth2Controller : VersionNeutralApiController
{
    private readonly OAuth2Service _oauth2Service;

    public OAuth2Controller(OAuth2Service oauth2Service) => _oauth2Service = oauth2Service;

    [HttpGet("{provider}")]
    [AllowAnonymous]
    [TenantIdHeader]
    [SwaggerOperation("External authentication", "Supports Google and Facebook.")]
    public async Task<IActionResult> ExternalLogin([FromRoute] string provider)
        => RedirectIfNotSwagger(await _oauth2Service.ExternalAuthAsync(provider));

    [HttpGet("callback/{provider}")]
    [AllowAnonymous]
    [SwaggerOperation("External authentication callback", "Supports Google and Facebook.")]
    public async Task<IActionResult> ExternalLoginCallback(
        [FromRoute] string provider,
        [FromQuery] string code,
        [FromQuery] string? error = "",
        [FromQuery] string? state = "")
        => RedirectIfNotSwagger(await _oauth2Service.ExternalAuthCallbackAsync(provider, code, error, state));
}