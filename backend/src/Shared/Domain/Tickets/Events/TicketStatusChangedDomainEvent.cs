using Shared.Domain.Base;

namespace Shared.Domain.Tickets.Events;

public sealed record TicketStatusChangedDomainEvent(
    Guid TicketId,
    Guid CompanyId,
    Guid ChangedByUserId,
    TicketStatus OldStatus,
    TicketStatus NewStatus)
    : IDomainEvent;