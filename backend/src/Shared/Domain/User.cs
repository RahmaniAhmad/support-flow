using Shared.Domain.Base;

namespace Shared.Domain;

public sealed class User : AggregateRoot
{
    public Guid CompanyId { get; set; }

    public Company Company { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Role { get; set; } = null!;
    public DateTime CreatedAtUtc { get; set; }
}