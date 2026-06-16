using Shared.Domain.Tickets;
using Shared.Domain.Users;

namespace Api.Authorization;

public sealed class TicketAccessService : ITicketAccessService
{
    public bool CanAccessTicket(Guid userId, Ticket ticket, UserRole role)
    {
        if (role == UserRole.Admin)
            return true;

        return ticket.CreatedByUserId == userId
                || ticket.AssignedToUserId == userId;

    }
}