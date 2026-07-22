using Shared.Domain.Base;

namespace Shared.Domain.Tickets.Events;

public sealed record TicketReopenedDomainEvent(
    Guid TicketId, Guid CompanyId, Guid reopenedByUserId) : IDomainEvent;
