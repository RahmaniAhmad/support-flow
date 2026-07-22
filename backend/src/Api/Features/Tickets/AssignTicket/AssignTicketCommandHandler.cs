using Api.Authorization;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace Api.Features.Tickets.AssignTicket;

public sealed class AssignTicketCommandHandler
    : IRequestHandler<AssignTicketCommand>
{
    private readonly SupportFlowDbContext _db;
    private readonly ITicketAccessService _accessService;

    public AssignTicketCommandHandler(SupportFlowDbContext db, ITicketAccessService accessService)
    {
        _db = db;
        _accessService = accessService;
    }

    public async Task Handle(
        AssignTicketCommand request,
        CancellationToken cancellationToken)
    {
        var ticket = await _db.Tickets
            .FirstOrDefaultAsync(
                x =>
                    x.Id == request.TicketId,
                cancellationToken);

        if (ticket is null)
            throw new InvalidOperationException("Ticket not found.");

        var assignedUser = await _db.Users
            .FirstOrDefaultAsync(
                x => x.Id == request.AssignedToUserId,
                cancellationToken);


        if (assignedUser is null)
            throw new InvalidOperationException(
                "Assigned user not found.");

        if (!_accessService.CanAssignTicket(
                ticket,
                assignedUser))
        {
            throw new UnauthorizedAccessException();
        }

        ticket.AssignTo(request.AssignedToUserId);

        await _db.SaveChangesAsync(cancellationToken);
    }
}
