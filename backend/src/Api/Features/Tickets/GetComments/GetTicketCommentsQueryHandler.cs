using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Authentication;

namespace Api.Features.Tickets.GetComments;

public sealed class GetTicketCommentsQueryHandler
    : IRequestHandler<GetTicketCommentsQuery, List<GetTicketCommentsResponse>?>
{
    private readonly SupportFlowDbContext _db;
    private readonly ICurrentUser _currentUser;

    public GetTicketCommentsQueryHandler(
        SupportFlowDbContext db,
        ICurrentUser currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<List<GetTicketCommentsResponse>?> Handle(
        GetTicketCommentsQuery request,
        CancellationToken cancellationToken)
    {
        var ticketExists = await _db.Tickets
            .AnyAsync(x =>
                x.Id == request.TicketId &&
                x.CompanyId == _currentUser.CompanyId,
                cancellationToken);

        if (!ticketExists)
            return null;

        return await _db.TicketComments
            .Where(x => x.TicketId == request.TicketId)
            .OrderBy(x => x.CreatedAtUtc)
            .Select(x => new GetTicketCommentsResponse(
                x.Id,
                x.Content,
                x.CreatedAtUtc))
            .ToListAsync(cancellationToken);
    }
}