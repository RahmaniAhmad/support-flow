using Shared.Domain.Base;

namespace Shared.Domain.Companies;

public sealed class Company : AggregateRoot
{
    public string Name { get; private set; } = string.Empty;

    public string? Description { get; private set; }

    public string? Website { get; private set; }

    public string? Phone { get; private set; }

    public DateTime CreatedAtUtc { get; private set; }


    private Company() { }


    public static Company Create(string name)
    {
        ValidateName(name);

        return new Company
        {
            Name = name.Trim(),
            CreatedAtUtc = DateTime.UtcNow
        };
    }


    public void Rename(string name)
    {
        ValidateName(name);

        Name = name.Trim();
    }


    public void UpdateDetails(
        string? description,
        string? website,
        string? phone)
    {
        Description = Normalize(description);
        Website = Normalize(website);
        Phone = Normalize(phone);
    }


    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new InvalidOperationException(
                "Company name is required.");
    }

    private static string? Normalize(string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? null
            : value.Trim();
    }
}