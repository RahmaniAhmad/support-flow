namespace Shared.Domain;

public sealed class TicketComment
{
    public Guid Id { get; set; }

    public Guid TicketId { get; set; }

    public Guid AuthorUserId { get; set; }

    public string Content { get; set; } = string.Empty;

    public DateTime CreatedAtUtc { get; set; }

    public Ticket Ticket { get; set; } = null!;
}