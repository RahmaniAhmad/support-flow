using Shared.Domain.Exceptions;

namespace Shared.Domain.Tickets.Exceptions;

public sealed class InvalidTicketTransitionException(
    TicketStatus currentStatus,
    TicketStatus targetStatus)
    : DomainException(
        $"Cannot transition ticket from {currentStatus} to {targetStatus}.");