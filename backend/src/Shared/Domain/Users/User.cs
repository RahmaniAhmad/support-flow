using Shared.Domain.Base;
using Shared.Domain.Users;

namespace Shared.Domain;

public sealed class User : AggregateRoot
{
    public Guid CompanyId { get; private set; }

    public string Email { get; private set; } = string.Empty;

    public string PasswordHash { get; private set; } = string.Empty;

    public UserRole Role { get; private set; }

    public DateTime CreatedAtUtc { get; private set; }

    private User() { }

    public static User Create(
        Guid companyId,
        string email,
        string passwordHash,
        UserRole role)
    {
        var user = new User
        {
            CompanyId = companyId,
            Email = email,
            PasswordHash = passwordHash,
            Role = role,
            CreatedAtUtc = DateTime.UtcNow
        };

        return user;
    }
}