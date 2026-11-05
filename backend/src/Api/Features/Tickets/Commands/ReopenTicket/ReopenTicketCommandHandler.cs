using Api.Authorization;
using Api.Exceptions;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Authentication;

namespace Api.Features.Tickets.Commands.ReopenTicket;

public sealed class ReopenTicketCommandHandler
    : IRequestHandler<ReopenTicketCommand>
{
    private readonly SupportFlowDbContext _db;
    private readonly ICurrentUser _currentUser;
    private readonly ITicketAccessService _accessService;

    public ReopenTicketCommandHandler(SupportFlowDbContext db, ICurrentUser currentUser, ITicketAccessService accessService)
    {
        _db = db;
        _currentUser = currentUser;
        _accessService = accessService;
    }

    public async Task Handle(
        ReopenTicketCommand request,
        CancellationToken cancellationToken)
    {
        var ticket = await _accessService
        .ApplyTicketAccessFilter(_db.Tickets)
        .FirstOrDefaultAsync(
           x => x.Id == request.TicketId,
           cancellationToken);

        if (ticket is null)
        {
            throw new NotFoundException(
                TicketErrors.NotFoundMessage,
                TicketErrors.NotFoundCode);
        }

        if (!_accessService.CanReopenTicket(ticket))
        {
            throw new ForbiddenException(
                TicketErrors.CannotReopenTicketMessage,
                TicketErrors.CannotReopenTicketCode);
        }

        ticket.Reopen(_currentUser.UserId);

        await _db.SaveChangesAsync(cancellationToken);
    }
}
