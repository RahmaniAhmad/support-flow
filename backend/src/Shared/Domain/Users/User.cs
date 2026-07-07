using Shared.Domain.Base;
using Shared.Domain.Users;

namespace Shared.Domain;

public sealed class User : AggregateRoot
{
    private readonly List<RefreshToken> _refreshTokens = [];

    public Guid CompanyId { get; private set; }

    public string Email { get; private set; } = string.Empty;

    public string PasswordHash { get; private set; } = string.Empty;

    public UserRole Role { get; private set; }

    public DateTime CreatedAtUtc { get; private set; }

    public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens;

    private User() { }

    public static User Create(
           Guid companyId,
           string email,
           string passwordHash,
           UserRole role)
    {
        return new User
        {
            CompanyId = companyId,
            Email = email,
            PasswordHash = passwordHash,
            Role = role,
            CreatedAtUtc = DateTime.UtcNow
        };
    }

    public RefreshToken IssueRefreshToken(
        string tokenHash,
        DateTime expiresAtUtc)
    {
        return CreateRefreshToken(
            tokenHash,
            expiresAtUtc);
    }


    public RefreshToken RotateRefreshToken(
    RefreshToken currentToken,
    string newTokenHash,
    DateTime expiresAtUtc)
    {
        if (!_refreshTokens.Contains(currentToken))
        {
            throw new InvalidOperationException(
                "The refresh token does not belong to this user.");
        }

        currentToken.Revoke();

        return CreateRefreshToken(
            newTokenHash,
            expiresAtUtc);
    }

    private RefreshToken CreateRefreshToken(
        string tokenHash,
        DateTime expiresAtUtc)
    {
        var token = RefreshToken.Create(
          Id,
          tokenHash,
          expiresAtUtc);

        _refreshTokens.Add(token);

        return token;
    }
}