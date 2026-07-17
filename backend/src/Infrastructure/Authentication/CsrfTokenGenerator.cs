using System.Security.Cryptography;

namespace Infrastructure.Authentication;

public static class CsrfTokenGenerator
{
    public static string Generate()
    {
        return Convert.ToHexString(
            RandomNumberGenerator.GetBytes(32));
    }
}