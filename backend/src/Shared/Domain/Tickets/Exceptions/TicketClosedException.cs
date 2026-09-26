using Shared.Domain.Exceptions;

namespace Shared.Domain.Tickets.Exceptions;

public sealed class TicketClosedException()
    : DomainException(
        "Cannot perform this operation on a closed ticket.");