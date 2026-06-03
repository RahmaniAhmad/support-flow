namespace Shared.Domain;

public sealed class Ticket
{
    public Guid Id { get; set; }

    public Guid CompanyId { get; set; }

    public Guid CreatedByUserId { get; set; }

    public string Subject { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Status { get; set; } = "Open";
}