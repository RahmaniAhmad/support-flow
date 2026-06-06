namespace Shared.Domain;

public sealed class Ticket
{
    public Guid Id { get; set; }

    public Guid CompanyId { get; set; }

    public Guid CreatedByUserId { get; set; }
    public Guid? AssignedToUserId { get; set; }

    public string Subject { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public TicketStatus Status { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime? UpdatedAtUtc { get; set; }
}