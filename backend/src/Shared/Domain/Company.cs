using Shared.Domain.Base;

namespace Shared.Domain;

public sealed class Company : AggregateRoot
{
    public string Name { get; set; } = null!;

    public DateTime CreatedAtUtc { get; set; }

    public ICollection<User> Users { get; set; } = [];

}