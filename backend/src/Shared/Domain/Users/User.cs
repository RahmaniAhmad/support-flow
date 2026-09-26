using Shared.Domain.Base;
using Shared.Domain.Users.Exceptions;

namespace Shared.Domain.Users;

public sealed class User : AggregateRoot
{

    private readonly List<RefreshToken> _refreshTokens = [];
    private readonly List<PasswordResetToken> _passwordResetTokens = [];

    public Guid? CompanyId { get; private set; }

    public string Email { get; private set; } = string.Empty;

    public string PasswordHash { get; private set; } = string.Empty;

    public string FirstName { get; private set; } = string.Empty;

    public string LastName { get; private set; } = string.Empty;

    public string? Phone { get; private set; }

    public UserRole Role { get; private set; }

    public bool IsActive { get; private set; }

    public DateTime CreatedAtUtc { get; private set; }

    public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens;
    public IReadOnlyCollection<PasswordResetToken> PasswordResetTokens =>
        _passwordResetTokens;

    private User() { }

    public static User Create(
           Guid? companyId,
           string email,
           string passwordHash,
           UserRole role)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(email);
        ArgumentException.ThrowIfNullOrWhiteSpace(passwordHash);

        ValidateCompany(role, companyId);

        return new User
        {
            CompanyId = companyId,
            Email = email.Trim().ToLowerInvariant(),
            PasswordHash = passwordHash,
            Role = role,
            IsActive = true,
            CreatedAtUtc = DateTime.UtcNow
        };
    }

    public void UpdateProfile(
        string firstName,
        string lastName,
        string? phone)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(firstName);
        ArgumentException.ThrowIfNullOrWhiteSpace(lastName);

        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        Phone = phone?.Trim();
    }

    public void Deactivate()
    {
        if (!IsActive)
            return;

        IsActive = false;

        RevokeActiveRefreshTokens();
    }


    public void Activate()
    {
        if (IsActive)
            return;

        IsActive = true;
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
        ArgumentNullException.ThrowIfNull(currentToken);

        if (!_refreshTokens.Contains(currentToken))
        {
            throw new RefreshTokenNotOwnedByUserException();
        }

        currentToken.Revoke();

        return CreateRefreshToken(
            newTokenHash,
            expiresAtUtc);
    }

    public PasswordResetToken CreatePasswordResetToken(
        string tokenHash,
        DateTime expiresAtUtc)
    {
        var token = PasswordResetToken.Create(
            Id,
            tokenHash,
            expiresAtUtc);

        _passwordResetTokens.Add(token);

        return token;
    }

    public void ChangePassword(string passwordHash)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(passwordHash);

        PasswordHash = passwordHash;

        RevokeActiveRefreshTokens();
    }

    private static void ValidateCompany(
           UserRole role,
           Guid? companyId)
    {
        if (role == UserRole.SuperAdmin)
            return;


        if (companyId is null)
        {
            throw new CompanyRequiredForUserRoleException(role);

        }

        ArgumentOutOfRangeException.ThrowIfEqual(
         companyId.Value,
         Guid.Empty);
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

    private void RevokeActiveRefreshTokens()
    {
        foreach (var token in _refreshTokens)
        {
            if (token.IsActive)
            {
                token.Revoke();
            }
        }
    }
}