using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Tickets.ReopenTicket;

public sealed class ReopenTicketCommandHandler
    : IRequestHandler<ReopenTicketCommand>
{
    private readonly SupportFlowDbContext _db;

    public ReopenTicketCommandHandler(SupportFlowDbContext db)
    {
        _db = db;
    }

    public async Task Handle(
        ReopenTicketCommand request,
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

        ticket.Reopen();

        await _db.SaveChangesAsync(cancellationToken);
    }
}
