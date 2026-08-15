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

        var query =
            from user in _db.Users

            where user.Role == UserRole.Agent

            where !request.CompanyId.HasValue ||
                  user.CompanyId == request.CompanyId.Value

            join ticket in _db.Tickets
                on user.Id equals ticket.AssignedToUserId
                into tickets


            select new
            {
                user.Id,

                Name =
                    user.FirstName + " " +
                    user.LastName,


                AssignedTickets =
                    tickets.Count(),


                ResolvedTickets =
                    tickets.Count(x =>
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