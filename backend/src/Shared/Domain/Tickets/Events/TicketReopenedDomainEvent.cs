using Shared.Domain.Base;

namespace Shared.Domain.Tickets.Events;

public sealed record TicketReopenedDomainEvent(
    Guid TicketId) : IDomainEvent;
