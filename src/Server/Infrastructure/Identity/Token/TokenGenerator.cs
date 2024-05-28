using System.Security.Cryptography;

namespace MultiMart.Infrastructure.Identity.Token;

public static class TokenGenerator
{
    public static string GenerateToken()
    {
        byte[] randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        string token = Convert.ToBase64String(randomNumber);
        return token.Replace('+', '-').Replace('/', '_').TrimEnd('=');
    }
}