using Shared.Domain.Base;

namespace Shared.Domain.Tickets.Events;

public sealed record TicketPendingDomainEvent(
    Guid TicketId, Guid CompanyId, Guid MovedToPendingByUserId) : IDomainEvent;
