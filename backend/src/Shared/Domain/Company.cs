using Shared.Domain.Base;

namespace Shared.Domain;

public sealed class Company : AggregateRoot
{
    public string Name { get; private set; } = string.Empty;

    public DateTime CreatedAtUtc { get; private set; }

    private Company() { }

    public static Company Create(string name)
    {
        return new Company
        {
            Id = Guid.NewGuid(),
            Name = name,
            CreatedAtUtc = DateTime.UtcNow
        };
    }

}