namespace Shared.Domain;

public sealed class Company
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime CreatedAtUtc { get; set; }

    public ICollection<User> Users { get; set; } = [];

}