using Api.Authorization;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Tickets.GetTicket;

public sealed class GetTicketQueryHandler
    : IRequestHandler<GetTicketQuery, GetTicketResponse?>
{
    private readonly SupportFlowDbContext _db;
    private readonly ITicketAccessService _accessService;


    public GetTicketQueryHandler(
        SupportFlowDbContext db,
        ITicketAccessService accessService)
    {
        _db = db;
        _accessService = accessService;
    }

    public async Task<GetTicketResponse?> Handle(
        GetTicketQuery request,
        CancellationToken cancellationToken)
    {
        var ticket = await _accessService
       .ApplyTicketAccessFilter(_db.Tickets.AsNoTracking())
       .Where(x => x.Id == request.TicketId)
       .Select(x => new GetTicketResponse(
           x.Id,
           x.TicketNumber,
           x.Subject,
           x.Description,
           x.Status,
           x.AssignedToUserId,

           x.AssignedToUserId != null
               ? _db.Users
                   .Where(u => u.Id == x.AssignedToUserId)
                   .Select(u => u.Email)
                   .FirstOrDefault()
               : null,

           _db.Users
               .Where(u => u.Id == x.CreatedByUserId)
               .Select(u => u.Email)
               .First(),

           x.CreatedAtUtc,
           x.UpdatedAtUtc
       ))
       .FirstOrDefaultAsync(cancellationToken);

        if (ticket is null)
            throw new InvalidOperationException("Ticket not found");

        return ticket;
    }
}