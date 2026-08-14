using Shared.Domain.Base;

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
        if (userId == Guid.Empty)
            throw new InvalidOperationException(
                "User is required.");

        if (string.IsNullOrWhiteSpace(tokenHash))
            throw new InvalidOperationException(
                "Token hash is required.");

        if (expiresAtUtc <= DateTime.UtcNow)
            throw new InvalidOperationException(
                "Token expiration must be in the future.");

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
            throw new InvalidOperationException(
                "Password reset token has already been used.");

        UsedAtUtc = DateTime.UtcNow;
    }
}