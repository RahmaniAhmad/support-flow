
using Shared.Domain.Base;
using Shared.Domain.Tickets.Events;

namespace Shared.Domain.Tickets;

public sealed class TicketCategory : AggregateRoot
{
    public Guid CompanyId { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public string? Description { get; private set; }

    public bool IsActive { get; private set; }

    public DateTime CreatedAtUtc { get; private set; }

    public DateTime? UpdatedAtUtc { get; private set; }

    private TicketCategory()
    {
    }

    public static TicketCategory Create(
        Guid companyId,
        string name,
        string? description = null)
    {
        ArgumentOutOfRangeException.ThrowIfEqual(
            companyId,
            Guid.Empty);

        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        var category = new TicketCategory
        {
            CompanyId = companyId,
            Name = name.Trim(),
            Description = Normalize(description),
            IsActive = true,
            CreatedAtUtc = DateTime.UtcNow
        };

        category.AddDomainEvent(
            new TicketCategoryUpdatedDomainEvent(
                category.Id,
                category.CompanyId));

        return category;
    }

    public void UpdateDetails(
        string name,
        string? description)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        Name = name.Trim();
        Description = Normalize(description);
        UpdatedAtUtc = DateTime.UtcNow;

        AddDomainEvent(
            new TicketCategoryUpdatedDomainEvent(
                Id,
                CompanyId));
    }

    public void Activate()
    {
        if (IsActive)
        {
            return;
        }

        IsActive = true;
        UpdatedAtUtc = DateTime.UtcNow;

        AddDomainEvent(
            new TicketCategoryUpdatedDomainEvent(
                Id,
                CompanyId));
    }

    public void Deactivate()
    {
        if (!IsActive)
        {
            return;
        }

        IsActive = false;
        UpdatedAtUtc = DateTime.UtcNow;

        AddDomainEvent(
            new TicketCategoryUpdatedDomainEvent(
                Id,
                CompanyId));
    }

    private static string? Normalize(string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? null
            : value.Trim();
    }
}
