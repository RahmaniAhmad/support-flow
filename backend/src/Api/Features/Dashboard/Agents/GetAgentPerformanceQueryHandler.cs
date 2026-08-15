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


    public GetAgentPerformanceQueryHandler(
        SupportFlowDbContext db)
    {
        _db = db;
    }



    public async Task<IReadOnlyList<AgentPerformanceResponse>> Handle(
        GetAgentPerformanceQuery request,
        CancellationToken cancellationToken)
    {

        var users = _db.Users
            .AsQueryable();


        if (request.CompanyId.HasValue)
        {
            users = users.Where(x =>
                x.CompanyId == request.CompanyId.Value);
        }



        var result = await users

            .Where(x =>
                x.Role == UserRole.Agent)

            .Select(user =>
                new AgentPerformanceResponse(

                    user.Id,

                    user.FirstName + " " +
                    user.LastName,


                    _db.Tickets.Count(ticket =>
                        ticket.AssignedToUserId ==
                        user.Id),


                    _db.Tickets.Count(ticket =>
                        ticket.AssignedToUserId ==
                        user.Id &&
                        ticket.Status ==
                        TicketStatus.Resolved)
                ))

            .OrderByDescending(x =>
                x.AssignedTickets)

            .ToListAsync(cancellationToken);



        return result;
    }
}