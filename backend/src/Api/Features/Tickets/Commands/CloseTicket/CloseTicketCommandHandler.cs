using Api.Authorization;
using Api.Exceptions;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Authentication;

namespace Api.Features.Tickets.Commands.CloseTicket;

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

        if (!_accessService.CanCloseTicket(ticket))
        {
            throw new ForbiddenException(
                TicketErrors.CannotCloseTicketMessage,
                TicketErrors.CannotCloseTicketCode);
        }


        ticket.Close(_currentUser.UserId);

        await _db.SaveChangesAsync(cancellationToken);
    }
}
