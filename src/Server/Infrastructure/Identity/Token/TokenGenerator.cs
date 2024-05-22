using System.Security.Cryptography;

namespace BookStack.Infrastructure.Identity.Token;

public class TokenGenerator
{
    public static string GenerateToken()
    {
        byte[] randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}