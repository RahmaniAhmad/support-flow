using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Authentication;
using Shared.Contracts;

namespace Api.Features.Tickets.GetTickets;

public sealed class GetTicketsQueryHandler
    : IRequestHandler<GetTicketsQuery, PagedResult<GetTicketsResponse>>
{
    private readonly SupportFlowDbContext _db;
    private readonly ICurrentUser _currentUser;

    public GetTicketsQueryHandler(
        SupportFlowDbContext db,
        ICurrentUser currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<PagedResult<GetTicketsResponse>> Handle(
        GetTicketsQuery request,
        CancellationToken cancellationToken)
    {

        var query = _db.Tickets
        .AsNoTracking()
        .Where(x => x.CompanyId == _currentUser.CompanyId);


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

        if (request.AssignedToUserId is not null)
        {
            query = query.Where(x =>
                x.AssignedToUserId == request.AssignedToUserId);
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