using Shared.Domain.Base;
using Shared.Domain.Users;

namespace Shared.Domain;

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
        ValidateCompany(role, companyId);
        ValidateEmail(email);
        return new User
        {
            CompanyId = companyId,
            Email = email,
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
        if (string.IsNullOrWhiteSpace(firstName))
            throw new InvalidOperationException("First name is required.");

        if (string.IsNullOrWhiteSpace(lastName))
            throw new InvalidOperationException("Last name is required.");

        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        Phone = phone?.Trim();
    }

    public void Deactivate()
    {
        if (!IsActive)
            return;

        IsActive = false;

        foreach (var token in _refreshTokens)
        {
            token.Revoke();
        }
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
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new InvalidOperationException(
                "Password hash is required.");

        PasswordHash = passwordHash;

        foreach (var token in _refreshTokens)
        {
            token.Revoke();
        }
    }

    private static void ValidateCompany(
           UserRole role,
           Guid? companyId)
    {
        if (role == UserRole.SuperAdmin)
            return;


        if (companyId is null)
        {
            throw new InvalidOperationException(
                "Company is required for this user role.");
        }
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

    private static void ValidateEmail(string email)
    {

        if (string.IsNullOrWhiteSpace(email))
            throw new InvalidOperationException("Email is required.");

        if (!email.Contains('@'))
            throw new InvalidOperationException("Invalid email.");
    }
}