using Api.Authorization;
using Api.Exceptions;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Authentication;

namespace Api.Features.Tickets.Commands.ChangeTicketPriority;

public sealed class ChangeTicketPriorityCommandHandler
    : IRequestHandler<ChangeTicketPriorityCommand>
{
    private readonly SupportFlowDbContext _db;
    private readonly ICurrentUser _currentUser;
    private readonly ITicketAccessService _ticketAccessService;

    public ChangeTicketPriorityCommandHandler(
        SupportFlowDbContext db,
        ICurrentUser currentUser,
        ITicketAccessService ticketAccessService)
    {
        _db = db;
        _currentUser = currentUser;
        _ticketAccessService = ticketAccessService;
    }

    public async Task Handle(
        ChangeTicketPriorityCommand request,
        CancellationToken cancellationToken)
    {
        var ticket = await _ticketAccessService
            .ApplyTicketAccessFilter(_db.Tickets)
            .FirstOrDefaultAsync(
                x => x.Id == request.TicketId,
                cancellationToken)
            ?? throw new NotFoundException(
                "Ticket was not found.",
                "TICKET_NOT_FOUND");

        ticket.ChangePriority(
            request.Priority,
            _currentUser.UserId);

        await _db.SaveChangesAsync(cancellationToken);
    }
}
