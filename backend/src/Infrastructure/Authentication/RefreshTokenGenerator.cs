using System.Security.Cryptography;
using Shared.Authentication;

namespace Infrastructure.Authentication;

public sealed class RefreshTokenGenerator : IRefreshTokenGenerator
{
    public string Generate()
    {
        var bytes = RandomNumberGenerator.GetBytes(64);

        return Convert.ToBase64String(bytes);
    }
}