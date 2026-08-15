using System.Security.Cryptography;
using System.Text;
using Shared.Authentication;

namespace Infrastructure.Authentication;

public sealed class PasswordResetTokenGenerator
    : IPasswordResetTokenGenerator
{
    public string Generate()
    {
        var bytes = RandomNumberGenerator.GetBytes(32);

        return Convert.ToBase64String(bytes)
            .Replace("+", "-")
            .Replace("/", "_")
            .Replace("=", "");
    }

    public string Hash(string token)
    {
        var bytes = SHA256.HashData(
            Encoding.UTF8.GetBytes(token));

        return Convert.ToHexString(bytes);
    }
}