using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Tickets.ResolveTicket;

public sealed class ResolveTicketCommandHandler
    : IRequestHandler<ResolveTicketCommand>
{
    private readonly SupportFlowDbContext _db;

    public ResolveTicketCommandHandler(SupportFlowDbContext db)
    {
        _db = db;
    }

    public async Task Handle(
        ResolveTicketCommand request,
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

        ticket.Resolve();

        await _db.SaveChangesAsync(cancellationToken);
    }
}
