using BookStack.Infrastructure.Auth.OAuth2;
using Google.Apis.Oauth2.v2;

namespace BookStack.Host.Controllers.OAuth2;

public class OAuth2Controller : VersionNeutralApiController
{
    private readonly OAuth2Service _oauth2Service;

    public OAuth2Controller(OAuth2Service oauth2Service) => _oauth2Service = oauth2Service;

    [HttpGet("{provider}")]
    [AllowAnonymous]
    [TenantIdHeader]
    [OpenApiOperation("External authentication", "Supports Google and Facebook.")]
    public async Task<IActionResult> ExternalLogin([FromRoute] string provider)
        => HandleRedirect(await _oauth2Service.ExternalAuthAsync(provider));

    [HttpGet("callback/{provider}")]
    [AllowAnonymous]
    [OpenApiOperation("External authentication callback", "Supports Google and Facebook.")]
    public async Task<IActionResult> ExternalLoginCallback(
        [FromRoute] string provider,
        [FromQuery] string code,
        [FromQuery] string? error = "",
        [FromQuery] string? state = "")
        => HandleRedirect(await _oauth2Service.ExternalAuthCallbackAsync(provider, code, error, state));
}