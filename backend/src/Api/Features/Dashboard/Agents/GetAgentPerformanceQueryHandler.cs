using Api.Authorization;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Domain.Tickets;
using Shared.Domain.Users;

namespace Api.Features.Dashboard.Agents;


public sealed class GetAgentPerformanceQueryHandler
    : IRequestHandler<
        GetAgentPerformanceQuery,
        IReadOnlyList<AgentPerformanceResponse>>
{

    private readonly SupportFlowDbContext _db;
    private readonly ITicketAccessService _ticketAccess;


    public GetAgentPerformanceQueryHandler(
        SupportFlowDbContext db,
        ITicketAccessService ticketAccess)
    {
        _db = db;
        _ticketAccess = ticketAccess;
    }



    public async Task<IReadOnlyList<AgentPerformanceResponse>> Handle(
        GetAgentPerformanceQuery request,
        CancellationToken cancellationToken)
    {

        var tickets = _ticketAccess
            .ApplyTicketAccessFilter(_db.Tickets.AsNoTracking());

        if (request.CompanyId.HasValue)
        {
            tickets = tickets.Where(x =>
                x.CompanyId == request.CompanyId.Value);
        }

        var query =
            from ticket in tickets
            join user in _db.Users.AsNoTracking()
                on ticket.AssignedToUserId equals (Guid?)user.Id
            where user.Role == UserRole.Agent &&
                  user.CompanyId == ticket.CompanyId
            group ticket by new
            {
                user.Id,
                user.FirstName,
                user.LastName
            }
            into assignedTickets
            select new
            {
                Id = assignedTickets.Key.Id,
                Name = assignedTickets.Key.FirstName + " " +
                       assignedTickets.Key.LastName,
                AssignedTickets = assignedTickets.Count(),
                ResolvedTickets = assignedTickets.Count(x =>
                    x.Status == TicketStatus.Resolved)
            };



        var result = await query

            .OrderByDescending(x =>
                x.AssignedTickets)

            .Select(x =>
                new AgentPerformanceResponse(
                    x.Id,
                    x.Name,
                    x.AssignedTickets,
                    x.ResolvedTickets))

            .ToListAsync(cancellationToken);



        return result;
    }
}