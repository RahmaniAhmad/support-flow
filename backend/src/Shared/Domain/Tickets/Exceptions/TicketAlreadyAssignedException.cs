using Shared.Domain.Exceptions;

namespace Shared.Domain.Tickets.Exceptions;

public sealed class TicketAlreadyAssignedException(
    Guid assignedToUserId)
    : DomainException(
        "The ticket is already assigned to this user.")
{
    public Guid AssignedToUserId { get; } = assignedToUserId;
}