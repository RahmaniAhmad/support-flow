using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Tickets.CloseTicket;

public sealed class CloseTicketCommandHandler
    : IRequestHandler<CloseTicketCommand>
{
    private readonly SupportFlowDbContext _db;

    public CloseTicketCommandHandler(SupportFlowDbContext db)
    {
        _db = db;
    }

    public async Task Handle(
        CloseTicketCommand request,
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

        ticket.Close();

        await _db.SaveChangesAsync(cancellationToken);
    }
}
