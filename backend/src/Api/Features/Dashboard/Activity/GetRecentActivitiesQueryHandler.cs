using Api.Authorization;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Api.Features.Dashboard.Activity;


public sealed class GetRecentActivitiesQueryHandler
    : IRequestHandler<
        GetRecentActivitiesQuery,
        IReadOnlyList<RecentActivityResponse>>
{

    private readonly SupportFlowDbContext _db;
    private readonly ITicketAccessService _ticketAccess;


    public GetRecentActivitiesQueryHandler(
        SupportFlowDbContext db,
        ITicketAccessService ticketAccess)
    {
        _db = db;
        _ticketAccess = ticketAccess;
    }



    public async Task<IReadOnlyList<RecentActivityResponse>> Handle(
        GetRecentActivitiesQuery request,
        CancellationToken cancellationToken)
    {


        var query = _ticketAccess
            .ApplyTicketAccessFilter(_db.Tickets.AsNoTracking());



        if (request.CompanyId.HasValue)
        {
            query = query.Where(x =>
                x.CompanyId ==
                request.CompanyId.Value);
        }



        var result = await query

            .OrderByDescending(x =>
                x.CreatedAtUtc)

            .Take(Math.Clamp(request.Limit, 1, 20))

            .Select(x =>
                new RecentActivityResponse(
                    $"Ticket #{x.TicketNumber} created",
                    x.CreatedAtUtc
                ))

            .ToListAsync(cancellationToken);



        return result;
    }
}