using MultiMart.Application.Common.Interfaces;
using MultiMart.Application.Identity.Tokens.Models;
using MultiMart.Application.Identity.Tokens.Requests;

namespace MultiMart.Application.Identity.Tokens.Interfaces;

public interface ITokenService : IScopedService
{
    Task<TokenResponse> GetTokenAsync(GetTokenRequest request, CancellationToken cancellationToken);
    Task<TokenResponse> RefreshTokenAsync(string ipAddress);
    Task<string> RevokeCurrentUserRefreshTokenAsync();
}