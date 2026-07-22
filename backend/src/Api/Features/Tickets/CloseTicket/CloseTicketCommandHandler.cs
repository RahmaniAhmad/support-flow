using Api.Authorization;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Authentication;

namespace Api.Features.Tickets.CloseTicket;

public sealed class CloseTicketCommandHandler
    : IRequestHandler<CloseTicketCommand>
{
    private readonly SupportFlowDbContext _db;
    private readonly ICurrentUser _currentUser;
    private readonly ITicketAccessService _accessService;

    public CloseTicketCommandHandler(SupportFlowDbContext db, ICurrentUser currentUser, ITicketAccessService accessService)
    {
        _db = db;
        _currentUser = currentUser;
        _accessService = accessService;
    }

    public async Task Handle(
        CloseTicketCommand request,
        CancellationToken cancellationToken)
    {
        var ticket = await _db.Tickets
            .FirstOrDefaultAsync(
                x =>
                    x.Id == request.TicketId,
                cancellationToken);

        if (ticket is null)
            throw new InvalidOperationException("Ticket not found.");

        if (!_accessService.CanAccessTicket(ticket))
            throw new UnauthorizedAccessException();

        ticket.Close(_currentUser.UserId);

        await _db.SaveChangesAsync(cancellationToken);
    }
}
