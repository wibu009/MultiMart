using MultiMart.Application.Identity.Tokens;
using MultiMart.Application.Identity.Tokens.Get;
using MultiMart.Application.Identity.Tokens.Refresh;
using MultiMart.Application.Identity.Tokens.Revoke;
using MultiMart.Infrastructure.Common.Extensions;
using MultiMart.Infrastructure.OpenApi;

namespace MultiMart.Host.Controllers.Identity;

public sealed class TokensController : VersionNeutralApiController
{
    [HttpPost]
    [AllowAnonymous]
    [TenantIdHeader]
    [SwaggerOperation("Request an access token using credentials or token.", "")]
    public async Task<TokenResponse> GetTokenAsync(
        GetTokenRequest request,
        CancellationToken cancellationToken)
    => await Mediator.Send(request.SetPropertyValue(nameof(request.IpAddress), Request.GetIpAddress()), cancellationToken);

    [HttpGet("refresh")]
    [AllowAnonymous]
    [TenantIdHeader]
    [SwaggerOperation("Request an access token using a refresh token.", "")]
    public async Task<TokenResponse> RefreshAsync()
    => await Mediator.Send(
        new RefreshTokenRequest
    {
        IpAddress = Request.GetIpAddress()
    });

    [HttpPost("revoke")]
    [SwaggerOperation("Revoke current user's refresh token.", "")]
    public async Task<IActionResult> RevokeAsync()
    => RedirectIfNotSwagger(await Mediator.Send(new RevokeRefreshTokenRequest()));
}