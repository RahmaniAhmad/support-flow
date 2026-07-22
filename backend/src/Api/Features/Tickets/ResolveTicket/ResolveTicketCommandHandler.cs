using Api.Authorization;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Authentication;

namespace Api.Features.Tickets.ResolveTicket;

public sealed class ResolveTicketCommandHandler
    : IRequestHandler<ResolveTicketCommand>
{
    private readonly SupportFlowDbContext _db;
    private readonly ICurrentUser _currentUser;
    private readonly ITicketAccessService _accessService;

    public ResolveTicketCommandHandler(SupportFlowDbContext db, ICurrentUser currentUser, ITicketAccessService accessService)
    {
        _db = db;
        _currentUser = currentUser;
        _accessService = accessService;
    }

    public async Task Handle(
        ResolveTicketCommand request,
        CancellationToken cancellationToken)
    {
        var ticket = await _db.Tickets
            .FirstOrDefaultAsync(
                x => x.Id == request.TicketId,
                cancellationToken);

        if (ticket is null)
            throw new InvalidOperationException("Ticket not found.");

        if (!_accessService.CanResolveTicket(ticket))
            throw new UnauthorizedAccessException();

        ticket.Resolve();

        await _db.SaveChangesAsync(cancellationToken);
    }
}
