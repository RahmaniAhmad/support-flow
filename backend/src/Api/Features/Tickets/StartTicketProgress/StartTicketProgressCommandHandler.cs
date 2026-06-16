using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Tickets.StartTicketProgress;

public sealed class StartTicketProgressCommandHandler
    : IRequestHandler<StartTicketProgressCommand>
{
    private readonly SupportFlowDbContext _db;

    public StartTicketProgressCommandHandler(SupportFlowDbContext db)
    {
        _db = db;
    }

    public async Task Handle(
        StartTicketProgressCommand request,
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

        ticket.StartProgress();

        await _db.SaveChangesAsync(cancellationToken);
    }
}
