using Shared.Domain.Exceptions;

namespace Shared.Domain.Tickets.Exceptions;

public sealed class TicketNotAssignedException()
    : DomainException(
        "Ticket must be assigned to an agent.");