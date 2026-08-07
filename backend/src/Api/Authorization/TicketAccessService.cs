using Shared.Authentication;
using Shared.Domain;
using Shared.Domain.Tickets;
using Shared.Domain.Users;

namespace Api.Authorization;

public sealed class TicketAccessService : ITicketAccessService
{
    private readonly ICurrentUser _currentUser;
    public TicketAccessService(
     ICurrentUser currentUser)
    {
        _currentUser = currentUser;
    }

    public bool CanAccessTicket(Ticket ticket)
    {
        return _currentUser.Role switch
        {
            UserRole.SuperAdmin =>
                true,


            UserRole.Admin =>
               IsSameCompany(ticket),


            UserRole.Agent =>
                IsSameCompany(ticket)
                &&
                (
                    ticket.AssignedToUserId == null
                    ||
                     IsAssignedAgent(ticket)
                ),


            UserRole.Customer =>
                ticket.CreatedByUserId == _currentUser.UserId,


            _ =>
                false
        };
    }

    public bool CanStartProgress(Ticket ticket)
    {
        return _currentUser.Role switch
        {
            UserRole.SuperAdmin => true,

            UserRole.Admin =>
                IsSameCompany(ticket),

            UserRole.Agent =>
                 IsSameCompany(ticket)
                &&
                IsAssignedAgent(ticket),

            _ => false
        };
    }

    public bool CanMoveToPending(Ticket ticket)
    {
        return _currentUser.Role switch
        {
            UserRole.SuperAdmin => true,

            UserRole.Admin =>
                IsSameCompany(ticket),

            UserRole.Agent =>
                 IsSameCompany(ticket)
                &&
                IsAssignedAgent(ticket),

            _ => false
        };
    }

    public bool CanAssignTicketTo(Ticket ticket, User assignedUser)
    {
        return _currentUser.Role switch
        {
            UserRole.SuperAdmin =>
            assignedUser.Role == UserRole.Admin
            ||
            assignedUser.Role == UserRole.Agent,

            UserRole.Admin =>
                IsSameCompany(ticket)
                &&
                assignedUser.CompanyId == _currentUser.CompanyId
                &&
                assignedUser.Role == UserRole.Agent,

            UserRole.Agent =>
                IsSameCompany(ticket)
                &&
                ticket.AssignedToUserId == null
                &&
                assignedUser.Id == _currentUser.UserId,

            _ =>
                false
        };
    }

    public bool CanResolveTicket(Ticket ticket)
    {
        return _currentUser.Role switch
        {
            UserRole.SuperAdmin => true,

            UserRole.Admin =>
                IsSameCompany(ticket),

            UserRole.Agent =>
                IsSameCompany(ticket)
                &&
                IsAssignedAgent(ticket),

            _ => false
        };
    }

    public bool CanCloseTicket(Ticket ticket)
    {
        return _currentUser.Role switch
        {
            UserRole.SuperAdmin => true,

            UserRole.Admin =>
                IsSameCompany(ticket),

            UserRole.Agent =>
                IsSameCompany(ticket)
                &&
                IsAssignedAgent(ticket),

            UserRole.Customer =>
                ticket.CreatedByUserId == _currentUser.UserId,

            _ => false
        };
    }

    public bool CanReopenTicket(Ticket ticket)
    {
        return _currentUser.Role switch
        {
            UserRole.SuperAdmin => true,

            UserRole.Admin =>
                IsSameCompany(ticket),

            UserRole.Agent =>
                IsSameCompany(ticket)
                &&
                IsAssignedAgent(ticket),

            UserRole.Customer =>
                ticket.CreatedByUserId == _currentUser.UserId,

            _ => false
        };
    }

    public bool CanComment(Ticket ticket)
    {
        return _currentUser.Role switch
        {
            UserRole.SuperAdmin => true,

            UserRole.Admin =>
                IsSameCompany(ticket),

            UserRole.Agent =>
                IsSameCompany(ticket)
                &&
                IsAssignedAgent(ticket),

            UserRole.Customer =>
                ticket.CreatedByUserId == _currentUser.UserId,

            _ => false
        };
    }

    public IQueryable<Ticket> ApplyTicketAccessFilter(
        IQueryable<Ticket> tickets)
    {
        return _currentUser.Role switch
        {
            UserRole.SuperAdmin =>
                tickets,


            UserRole.Admin =>
                tickets.Where(x =>
                    x.CompanyId == _currentUser.CompanyId),


            UserRole.Agent =>
                tickets.Where(x =>
                    x.CompanyId == _currentUser.CompanyId
                    &&
                    (
                        x.AssignedToUserId == null
                        ||
                        x.AssignedToUserId == _currentUser.UserId
                    )),


            UserRole.Customer =>
                tickets.Where(x =>
                    x.CreatedByUserId == _currentUser.UserId),


            _ =>
                tickets.Where(x => false)
        };
    }
    private bool IsSameCompany(Ticket ticket)
    {
        return ticket.CompanyId == _currentUser.CompanyId;
    }

    private bool IsAssignedAgent(Ticket ticket)
    {
        return ticket.AssignedToUserId == _currentUser.UserId;
    }

}