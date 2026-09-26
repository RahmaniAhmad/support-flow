using Shared.Domain.Base;

namespace Shared.Domain.Tickets;

public sealed class TicketComment : Entity
{
    public Guid TicketId { get; private set; }

    public Guid AuthorUserId { get; private set; }

    public string Content { get; private set; } = string.Empty;

    public DateTime CreatedAtUtc { get; private set; }

    private TicketComment() { }

    public static TicketComment Create(Guid ticketId, Guid authorUserId, string content)
    {
        ArgumentOutOfRangeException.ThrowIfEqual(
               ticketId,
               Guid.Empty);

        ArgumentOutOfRangeException.ThrowIfEqual(
            authorUserId,
            Guid.Empty);

        ArgumentException.ThrowIfNullOrWhiteSpace(content);

        return new TicketComment
        {
            TicketId = ticketId,
            AuthorUserId = authorUserId,
            Content = content.Trim(),
            CreatedAtUtc = DateTime.UtcNow
        };
    }
}
