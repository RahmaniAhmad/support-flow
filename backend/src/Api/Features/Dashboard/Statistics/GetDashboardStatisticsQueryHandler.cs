using Api.Authorization;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Domain.Tickets;

namespace Api.Features.Dashboard.Statistics;


public sealed class GetDashboardStatisticsQueryHandler
    : IRequestHandler<
        GetDashboardStatisticsQuery,
        DashboardStatisticsResponse>
{

    private readonly SupportFlowDbContext _db;
    private readonly ITicketAccessService _ticketAccess;

    public GetDashboardStatisticsQueryHandler(
        SupportFlowDbContext db,
        ITicketAccessService ticketAccess)
    {
        _db = db;
        _ticketAccess = ticketAccess;
    }

    public async Task<DashboardStatisticsResponse> Handle(
        GetDashboardStatisticsQuery request,
        CancellationToken cancellationToken)
    {

        var query = _ticketAccess
            .ApplyTicketAccessFilter(_db.Tickets.AsNoTracking());


        if (request.CompanyId.HasValue)
        {
            query = query.Where(x =>
                x.CompanyId == request.CompanyId.Value);
        }



        return new DashboardStatisticsResponse(

            await query.CountAsync(
                cancellationToken),

            await query.CountAsync(
                x => x.Status == TicketStatus.Open,
                cancellationToken),


            await query.CountAsync(
                x => x.Status == TicketStatus.Assigned,
                cancellationToken),


            await query.CountAsync(
                x => x.Status == TicketStatus.InProgress,
                cancellationToken),


            await query.CountAsync(
                x => x.Status == TicketStatus.Pending,
                cancellationToken),


            await query.CountAsync(
                x => x.Status == TicketStatus.Resolved,
                cancellationToken),


            await query.CountAsync(
                x => x.Status == TicketStatus.Reopened,
                cancellationToken),


            await query.CountAsync(
                x => x.Status == TicketStatus.Closed,
                cancellationToken),


            await query.CountAsync(
                x => x.AssignedToUserId == null,
                cancellationToken)
        );
    }
}