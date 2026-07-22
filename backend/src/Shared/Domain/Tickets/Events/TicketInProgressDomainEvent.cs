using Shared.Domain.Base;

namespace Shared.Domain.Tickets.Events;

public sealed record TicketInProgressDomainEvent(
    Guid TicketId, Guid CompanyId, Guid AssignedToUserId) : IDomainEvent;
