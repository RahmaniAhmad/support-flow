using Shared.Domain.Exceptions;

namespace Shared.Domain.Tickets.Exceptions;

public sealed class TicketNumberAlreadyAssignedException(
    long ticketNumber)
    : DomainException(
        "A ticket number has already been assigned.")
{
    public long TicketNumber { get; } = ticketNumber;
}