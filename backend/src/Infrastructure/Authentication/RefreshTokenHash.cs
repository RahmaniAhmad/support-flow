using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Authentication;

public static class RefreshTokenHash
{
    public static string Compute(string refreshToken)
    {
        var hash = SHA256.HashData(
            Encoding.UTF8.GetBytes(refreshToken));

        return Convert.ToHexString(hash);
    }
}