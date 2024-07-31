using MultiMart.Application.Common.Interfaces;
using MultiMart.Application.Identity.Tokens.Get;

namespace MultiMart.Application.Identity.Tokens;

public interface ITokenService : IScopedService
{
    Task<TokenResponse> GetTokenAsync(GetTokenRequest request, CancellationToken cancellationToken);
    Task<TokenResponse> RefreshTokenAsync(string ipAddress);
    Task<string> RevokeCurrentUserRefreshTokenAsync();
}