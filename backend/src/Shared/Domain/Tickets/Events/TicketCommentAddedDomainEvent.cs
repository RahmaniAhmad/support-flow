using Shared.Domain.Base;

namespace Shared.Domain.Tickets.Events;

public sealed record TicketCommentAddedDomainEvent(
    Guid TicketId,
    Guid UserId,
    string content) : IDomainEvent;
