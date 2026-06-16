using Shared.Domain.Base;

namespace Shared.Domain.Tickets.Events;

public sealed record TicketResolvedDomainEvent(
    Guid TicketId) : IDomainEvent;
