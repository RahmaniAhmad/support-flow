using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Domain.Tickets;


namespace Api.Features.Dashboard.Trend;


public sealed class GetTicketTrendQueryHandler
: IRequestHandler<
    GetTicketTrendQuery,
    IReadOnlyList<TicketTrendResponse>>
{


    private readonly SupportFlowDbContext _db;


    public GetTicketTrendQueryHandler(
        SupportFlowDbContext db)
    {
        _db = db;
    }



    public async Task<IReadOnlyList<TicketTrendResponse>> Handle(
        GetTicketTrendQuery request,
        CancellationToken cancellationToken)
    {


        var query = _db.Tickets.AsQueryable();



        if (request.CompanyId.HasValue)
        {
            query = query.Where(x =>
                x.CompanyId == request.CompanyId.Value);
        }



        var result = await query

        .Where(x =>
            DateOnly.FromDateTime(x.CreatedAtUtc)
                >= request.From &&
            DateOnly.FromDateTime(x.CreatedAtUtc)
                <= request.To)

        .GroupBy(x =>
            x.CreatedAtUtc.Date)

        .Select(x =>
            new TicketTrendResponse(
                DateOnly.FromDateTime(x.Key),
                x.Count(),
                x.Count(t =>
                    t.Status == TicketStatus.Resolved)
            ))

        .OrderBy(x => x.Date)

        .ToListAsync(cancellationToken);



        return result;

    }

}