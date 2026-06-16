using Shared.Domain.Base;

namespace Shared.Domain.Tickets.Events;

public sealed record TicketAssignedDomainEvent(
    Guid TicketId,
    Guid AssignedToUserId) : IDomainEvent;
