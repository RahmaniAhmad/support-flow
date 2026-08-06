using Shared.Domain.Base;

namespace Shared.Domain.Tickets.Events;

public sealed record TicketProgressStartedDomainEvent(
    Guid TicketId, Guid CompanyId, Guid StartedByUserId) : IDomainEvent;
