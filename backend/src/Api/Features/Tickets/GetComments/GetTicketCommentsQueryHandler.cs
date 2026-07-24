using Api.Authorization;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Authentication;

namespace Api.Features.Tickets.GetComments;

public sealed class GetTicketCommentsQueryHandler
    : IRequestHandler<GetTicketCommentsQuery, List<GetTicketCommentsResponse>?>
{
    private readonly SupportFlowDbContext _db;
    private readonly ITicketAccessService _accessService;

    public GetTicketCommentsQueryHandler(
        SupportFlowDbContext db,
          ITicketAccessService accessService)
    {
        _db = db;
        _accessService = accessService;
    }

    public async Task<List<GetTicketCommentsResponse>?> Handle(
        GetTicketCommentsQuery request,
        CancellationToken cancellationToken)
    {
        var ticketExists = await _accessService
          .ApplyTicketAccessFilter(_db.Tickets.AsNoTracking())
          .AnyAsync(
              x => x.Id == request.TicketId,
              cancellationToken);

        if (!ticketExists)
            return null;

        return await _db.TicketComments
            .Where(x => x.TicketId == request.TicketId)
            .OrderBy(x => x.CreatedAtUtc)
            .Select(x => new GetTicketCommentsResponse(
                  x.Id,
                x.Content,
                x.AuthorUserId,

                _db.Users
                    .Where(u => u.Id == x.AuthorUserId)
                    .Select(u => $"{u.FirstName} {u.LastName}")
                    .FirstOrDefault() ?? "Unknown",

                _db.Users
                    .Where(u => u.Id == x.AuthorUserId)
                    .Select(u => u.Email)
                    .FirstOrDefault() ?? "",

                x.CreatedAtUtc))
            .ToListAsync(cancellationToken);
    }
}