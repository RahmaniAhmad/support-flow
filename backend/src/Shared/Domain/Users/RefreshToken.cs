using Shared.Domain.Base;
using Shared.Domain.Users.Exceptions;

namespace Shared.Domain.Users;

public sealed class RefreshToken : Entity
{
    public Guid UserId { get; private set; }

    public User User { get; private set; } = null!;

    public string TokenHash { get; private set; } = string.Empty;

    public DateTime CreatedAtUtc { get; private set; }

    public DateTime ExpiresAtUtc { get; private set; }

    public DateTime? RevokedAtUtc { get; private set; }

    private RefreshToken()
    {
    }

    public bool IsActive =>
        RevokedAtUtc is null &&
        ExpiresAtUtc > DateTime.UtcNow;

    public bool IsExpired =>
        ExpiresAtUtc <= DateTime.UtcNow;

    public static RefreshToken Create(
        Guid userId,
        string tokenHash,
        DateTime expiresAtUtc)
    {
        ArgumentOutOfRangeException.ThrowIfEqual(
            userId,
            Guid.Empty);

        ArgumentException.ThrowIfNullOrWhiteSpace(tokenHash);

        return new RefreshToken
        {
            UserId = userId,
            TokenHash = tokenHash,
            CreatedAtUtc = DateTime.UtcNow,
            ExpiresAtUtc = expiresAtUtc
        };
    }

    public void Revoke()
    {
        if (RevokedAtUtc is not null)
            throw new RefreshTokenAlreadyRevokedException();

        RevokedAtUtc = DateTime.UtcNow;
    }
}