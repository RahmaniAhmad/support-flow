using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Tickets.AssignTicket;

public sealed class AssignTicketCommandHandler
    : IRequestHandler<AssignTicketCommand>
{
    private readonly SupportFlowDbContext _db;

    public AssignTicketCommandHandler(SupportFlowDbContext db)
    {
        _db = db;
    }

    public async Task Handle(
        AssignTicketCommand request,
        CancellationToken cancellationToken)
    {
        var ticket = await _db.Tickets
            .FirstOrDefaultAsync(
                x =>
                    x.Id == request.TicketId &&
                    x.CompanyId == request.CompanyId,
                cancellationToken);

        if (ticket is null)
            throw new InvalidOperationException("Ticket not found.");

        ticket.AssignTo(request.AssignedToUserId);

        await _db.SaveChangesAsync(cancellationToken);
    }
}
