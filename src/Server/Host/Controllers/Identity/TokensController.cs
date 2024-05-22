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
    [OpenApiOperation("Request an access token using sign-in code or cookie.", "")]
    [ApiConventionMethod(typeof(FSHApiConventions), nameof(FSHApiConventions.Search))]
    public Task<TokenResponse> RefreshAsync([FromQuery] string? signInCode)
    {
        return signInCode == null
            ? _tokenService.RefreshTokenAsync(Request.GetIpAddress())
            : _tokenService.RefreshTokenAsync(Request.GetIpAddress(), signInCode);
    }
}