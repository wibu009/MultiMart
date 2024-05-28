namespace BookStack.Application.Identity.Tokens;

public interface ITokenService : ITransientService
{
    Task<TokenResponse> GetTokenAsync(TokenRequest request, string ipAddress, CancellationToken cancellationToken);
    Task<TokenResponse> GetTokenAsync(string? token, string ipAddress, CancellationToken cancellationToken);
    Task<TokenResponse> RefreshTokenAsync(string ipAddress);
    Task<string> RevokeCurrentUserRefreshTokenAsync();
}