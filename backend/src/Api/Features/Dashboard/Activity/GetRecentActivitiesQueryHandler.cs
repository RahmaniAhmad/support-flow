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


    public GetRecentActivitiesQueryHandler(
        SupportFlowDbContext db)
    {
        _db = db;
    }



    public async Task<IReadOnlyList<RecentActivityResponse>> Handle(
        GetRecentActivitiesQuery request,
        CancellationToken cancellationToken)
    {


        var query = _db.Tickets
            .AsQueryable();



        if (request.CompanyId.HasValue)
        {
            query = query.Where(x =>
                x.CompanyId ==
                request.CompanyId.Value);
        }



        var result = await query

            .OrderByDescending(x =>
                x.CreatedAtUtc)

            .Take(request.Limit)

            .Select(x =>
                new RecentActivityResponse(
                    $"Ticket #{x.TicketNumber} created",
                    x.CreatedAtUtc
                ))

            .ToListAsync(cancellationToken);



        return result;
    }
}