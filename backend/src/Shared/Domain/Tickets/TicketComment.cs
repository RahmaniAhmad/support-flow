using Shared.Domain.Base;

namespace Shared.Domain.Tickets;

public sealed class TicketComment : Entity
{
    public Guid TicketId { get; set; }

    public Guid AuthorUserId { get; set; }

    public string Content { get; set; } = string.Empty;

    public DateTime CreatedAtUtc { get; set; }

    public Ticket Ticket { get; set; } = null!;
}
