using Api.Authorization;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Authentication;
using Shared.Domain.Users;

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
        .FirstOrDefaultAsync(
            x => x.Id == request.TicketId,
            cancellationToken);

        if (ticket is null)
            throw new InvalidOperationException("Ticket not found");

        var createdByEmail = await _db.Users
        .Where(x => x.Id == ticket.CreatedByUserId)
        .Select(x => x.Email)
        .FirstAsync(cancellationToken);


        var assigneeEmail = ticket.AssignedToUserId is not null
            ? await _db.Users
                .Where(x => x.Id == ticket.AssignedToUserId)
                .Select(x => x.Email)
                .FirstOrDefaultAsync(cancellationToken)
            : null;

        return new GetTicketResponse(
            ticket.Id,
            ticket.Subject,
            ticket.Description,
            ticket.Status,
            ticket.AssignedToUserId,
            assigneeEmail,
            createdByEmail,
            ticket.CreatedAtUtc,
            ticket.UpdatedAtUtc
  );
    }
}