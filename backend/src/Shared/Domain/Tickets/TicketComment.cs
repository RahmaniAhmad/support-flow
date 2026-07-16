using Shared.Domain.Base;

namespace Shared.Domain.Tickets;

public sealed class TicketComment : Entity
{
    public Guid TicketId { get; private set; }

    public Guid AuthorUserId { get; private set; }

    public string Content { get; private set; } = string.Empty;

    public DateTime CreatedAtUtc { get; private set; }

    public Ticket Ticket { get; private set; } = null!;

    private TicketComment() { }

    public static TicketComment Create(Guid ticketId, Guid authorUserId, string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException("Comment content cannot be empty.", nameof(content));

        return new TicketComment
        {
            TicketId = ticketId,
            AuthorUserId = authorUserId,
            Content = content,
            CreatedAtUtc = DateTime.UtcNow
        };
    }
}
