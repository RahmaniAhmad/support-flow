using Shared.Domain.Base;
using Shared.Domain.Users.Exceptions;

namespace Shared.Domain.Users;

public sealed class PasswordResetToken : Entity
{
    public Guid UserId { get; private set; }

    public string TokenHash { get; private set; } = string.Empty;

    public DateTime ExpiresAtUtc { get; private set; }

    public DateTime? UsedAtUtc { get; private set; }

    private PasswordResetToken()
    {
    }

    private PasswordResetToken(
        Guid userId,
        string tokenHash,
        DateTime expiresAtUtc)
    {
        UserId = userId;
        TokenHash = tokenHash;
        ExpiresAtUtc = expiresAtUtc;
    }

    public static PasswordResetToken Create(
        Guid userId,
        string tokenHash,
        DateTime expiresAtUtc)
    {
        ArgumentOutOfRangeException.ThrowIfEqual(
           userId,
           Guid.Empty);

        ArgumentException.ThrowIfNullOrWhiteSpace(tokenHash);

        return new PasswordResetToken(
            userId,
            tokenHash,
            expiresAtUtc);
    }

    public bool IsValid(DateTime utcNow)
    {
        return UsedAtUtc is null &&
               ExpiresAtUtc > utcNow;
    }

    public void MarkAsUsed()
    {
        if (UsedAtUtc is not null)
            throw new PasswordResetTokenAlreadyUsedException();

        UsedAtUtc = DateTime.UtcNow;
    }
}