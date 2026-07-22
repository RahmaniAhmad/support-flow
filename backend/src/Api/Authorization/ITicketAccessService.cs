using Shared.Domain;
using Shared.Domain.Tickets;

namespace Api.Authorization;

public interface ITicketAccessService
{
    bool CanAccessTicket(Ticket ticket);
    bool CanStartProgress(Ticket ticket);
    bool CanAssignTicket(Ticket ticket, User assignedUser);
    bool CanResolveTicket(Ticket ticket);
    bool CanCloseTicket(Ticket ticket);
    bool CanReopenTicket(Ticket ticket);
    bool CanComment(Ticket ticket);
    IQueryable<Ticket> ApplyTicketAccessFilter(IQueryable<Ticket> tickets);
}