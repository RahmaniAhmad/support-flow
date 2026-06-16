using Shared.Domain.Tickets;
using Shared.Domain.Users;

namespace Api.Authorization;

public interface ITicketAccessService
{
    bool CanAccessTicket(Guid userId, Ticket ticket, UserRole role);
}