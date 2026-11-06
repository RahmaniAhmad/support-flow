using Shared.Domain.Base;

namespace Shared.Domain.Tickets.Events;

public sealed record TicketPriorityChangedDomainEvent(
    Guid TicketId,
    Guid CompanyId,
    Guid ChangedByUserId,
    TicketPriority PreviousPriority,
    TicketPriority NewPriority,
    DateTime ChangedAtUtc) : IDomainEvent;
