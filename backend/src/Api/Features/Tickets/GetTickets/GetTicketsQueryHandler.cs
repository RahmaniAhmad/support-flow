using Api.Authorization;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts;
using Shared.Domain.Users;

namespace Api.Features.Tickets.GetTickets;

public sealed class GetTicketsQueryHandler
    : IRequestHandler<GetTicketsQuery, PagedResult<GetTicketsResponse>>
{
    private readonly SupportFlowDbContext _db;
    private readonly ITicketAccessService _accessService;

    public GetTicketsQueryHandler(
     SupportFlowDbContext db,
     ITicketAccessService accessService)
    {
        _db = db;
        _accessService = accessService;
    }

    public async Task<PagedResult<GetTicketsResponse>> Handle(
        GetTicketsQuery request,
        CancellationToken cancellationToken)
    {

        var query = _accessService
           .ApplyTicketAccessFilter(
               _db.Tickets.AsNoTracking());


        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            query = query.Where(x =>
                EF.Functions.ILike(x.Subject, $"%{request.Search}%") ||
                EF.Functions.ILike(x.Description, $"%{request.Search}%"));
        }

        if (request.Status is not null)
        {
            query = query.Where(x =>
                x.Status == request.Status);
        }

        query = request.SortBy?.ToLowerInvariant() switch
        {
            "subject" => request.Descending
                ? query.OrderByDescending(x => x.Subject)
                : query.OrderBy(x => x.Subject),

            "status" => request.Descending
                ? query.OrderByDescending(x => x.Status)
                : query.OrderBy(x => x.Status),

            _ => request.Descending
                ? query.OrderByDescending(x => x.CreatedAtUtc)
                : query.OrderBy(x => x.CreatedAtUtc)
        };

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(x => new GetTicketsResponse(
                x.Id,
                x.Subject,
                x.Description,
                x.Status,
                x.AssignedToUserId,
                x.CreatedAtUtc
                ))
                .ToListAsync(cancellationToken);

        return new PagedResult<GetTicketsResponse>(
            items,
            totalCount,
            request.Page,
            request.PageSize);

    }
}