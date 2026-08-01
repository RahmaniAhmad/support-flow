using Api.Authorization;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Authentication;
using Shared.Contracts;
using Shared.Domain.Tickets;

namespace Api.Features.Tickets.GetTickets;

public sealed class GetTicketsQueryHandler
    : IRequestHandler<GetTicketsQuery, PagedResult<GetTicketsResponse>>
{
    private readonly SupportFlowDbContext _db;
    private readonly ICurrentUser _currentUser;
    private readonly ITicketAccessService _accessService;
    public GetTicketsQueryHandler(
     SupportFlowDbContext db,
     ICurrentUser currentUser,
     ITicketAccessService accessService)
    {
        _db = db;
        _currentUser = currentUser;
        _accessService = accessService;
    }

    public async Task<PagedResult<GetTicketsResponse>> Handle(
        GetTicketsQuery request,
        CancellationToken cancellationToken)
    {

        var query = _accessService
           .ApplyTicketAccessFilter(
               _db.Tickets.AsNoTracking());

        query = request.View switch
        {
            TicketView.AssignedToMe =>
                query.Where(x => x.AssignedToUserId == _currentUser.UserId),

            TicketView.CreatedByMe =>
                query.Where(x => x.CreatedByUserId == _currentUser.UserId),

            _ => query
        };


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
                x.TicketNumber,
                x.Subject,
                x.Description,
                x.Status,
                x.AssignedToUserId != null
                ? _db.Users
                .Where(u => u.Id == x.AssignedToUserId)
                .Select(u => u.FirstName + " " + u.LastName)
                .FirstOrDefault()
                : null,
                _db.Companies
                .Where(c => c.Id == x.CompanyId)
                .Select(c => c.Name)
                .First(),
                 _db.Users
                .Where(u => u.Id == x.CreatedByUserId)
                .Select(u => u.FirstName + " " + u.LastName)
                .First(),
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