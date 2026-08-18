using Api.Authorization;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Authentication;
using Shared.Contracts;
using Shared.Contracts.Sorting;
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

        if (request.Filter is not null)
        {
            query = request.Filter switch
            {
                TicketFilter.Unassigned =>
                    query.Where(x => x.AssignedToUserId == null),

                TicketFilter.Open =>
                    query.Where(x => x.Status == TicketStatus.Open),

                TicketFilter.Assigned =>
                    query.Where(x => x.Status == TicketStatus.Assigned),

                TicketFilter.InProgress =>
                    query.Where(x => x.Status == TicketStatus.InProgress),

                TicketFilter.Pending =>
                    query.Where(x => x.Status == TicketStatus.Pending),

                TicketFilter.Resolved =>
                    query.Where(x => x.Status == TicketStatus.Resolved),

                TicketFilter.Reopened =>
                    query.Where(x => x.Status == TicketStatus.Reopened),

                TicketFilter.Closed =>
                    query.Where(x => x.Status == TicketStatus.Closed),

                _ => query
            };
        }

        query = request.SortBy?.ToLowerInvariant() switch
        {
            "subject" => request.SortDirection == SortDirection.Desc
                ? query.OrderByDescending(x => x.Subject)
                : query.OrderBy(x => x.Subject),

            "status" => request.SortDirection == SortDirection.Desc
                ? query.OrderByDescending(x => x.Status)
                : query.OrderBy(x => x.Status),

            _ => request.SortDirection == SortDirection.Desc
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
                x.AssignedToUserId,
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