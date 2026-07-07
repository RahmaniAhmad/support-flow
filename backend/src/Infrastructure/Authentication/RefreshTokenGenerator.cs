using System.Security.Cryptography;
using Microsoft.AspNetCore.WebUtilities;
using Shared.Authentication;

namespace Infrastructure.Authentication;

public sealed class RefreshTokenGenerator : IRefreshTokenGenerator
{
    public string Generate()
    {
        return WebEncoders.Base64UrlEncode(
            RandomNumberGenerator.GetBytes(64));
    }
}