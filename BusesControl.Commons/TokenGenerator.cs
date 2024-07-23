using System.Security.Cryptography;

namespace BusesControl.Commons;

public class TokenGenerator
{
    public static string GenerateToken(int length = 32)
    {
        byte[] randomNumber = new byte[length];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
        }

        return Convert.ToBase64String(randomNumber);
    }
}
