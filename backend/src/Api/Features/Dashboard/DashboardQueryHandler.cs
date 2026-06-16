using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Domain.Tickets;

namespace Api.Features.Dashboard;

public sealed class DashboardQueryHandler
    : IRequestHandler<DashboardQuery, DashboardResponse>
{
    private readonly SupportFlowDbContext _db;

    public DashboardQueryHandler(
        SupportFlowDbContext db)
    {
        _db = db;
    }

    public async Task<DashboardResponse> Handle(
        DashboardQuery request,
        CancellationToken cancellationToken)
    {
        var tickets = await _db.Tickets
                 .Where(x => x.CompanyId == request.CompanyId)
                 .Select(x => new
                 {
                     x.Status,
                     x.AssignedToUserId
                 })
                 .ToListAsync(cancellationToken);

        var openTickets = 0;
        var inProgressTickets = 0;
        var resolvedTickets = 0;
        var closedTickets = 0;
        var unassignedTickets = 0;

        foreach (var ticket in tickets)
        {
            switch (ticket.Status)
            {
                case TicketStatus.Open:
                    openTickets++;
                    break;

                case TicketStatus.InProgress:
                    inProgressTickets++;
                    break;

                case TicketStatus.Resolved:
                    resolvedTickets++;
                    break;

                case TicketStatus.Closed:
                    closedTickets++;
                    break;
            }

            if (ticket.AssignedToUserId is null)
                unassignedTickets++;
        }

        return new DashboardResponse(
                 openTickets,
                 inProgressTickets,
                 resolvedTickets,
                 closedTickets,
                 unassignedTickets);
    }
}