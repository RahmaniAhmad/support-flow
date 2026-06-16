using Shared.Domain.Base;

namespace Shared.Domain.Tickets.Events;

public sealed record TicketClosedDomainEvent(
    Guid TicketId) : IDomainEvent;
