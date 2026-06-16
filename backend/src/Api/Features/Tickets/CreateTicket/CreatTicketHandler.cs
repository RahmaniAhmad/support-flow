
using Infrastructure.Persistence;
using MediatR;
using Shared.Domain.Tickets;

namespace Api.Features.Tickets.CreateTicket;

public class CreateTicketCommandHandler : IRequestHandler<CreateTicketCommand, Guid>
{
    private readonly SupportFlowDbContext _db;

    public CreateTicketCommandHandler(SupportFlowDbContext db)
    {
        _db = db;
    }

    public async Task<Guid> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
    {
        var ticket = Ticket.Create(request.CompanyId, request.UserId, request.Subject, request.Description);
        _db.Tickets.Add(ticket);
        await _db.SaveChangesAsync(cancellationToken);

        return ticket.Id;
    }
}
