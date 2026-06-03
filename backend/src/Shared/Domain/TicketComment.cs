namespace Shared.Domain;

public sealed class TicketComment
{
    public Guid Id { get; set; }

    public Guid TicketId { get; set; }

    public Guid UserId { get; set; }

    public string Message { get; set; } = null!;
}