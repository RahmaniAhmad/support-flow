using Infrastructure.Persistence;
using MediatR;
using Shared.Authentication;
using Shared.Domain.Tickets;

namespace Api.Features.Tickets.CreateTicket;

public sealed class CreateTicketCommandHandler
    : IRequestHandler<CreateTicketCommand, Guid>
{
    private readonly SupportFlowDbContext _db;
    private readonly ICurrentUser _currentUser;


    public CreateTicketCommandHandler(
        SupportFlowDbContext db,
        ICurrentUser currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }


    public async Task<Guid> Handle(
        CreateTicketCommand request,
        CancellationToken cancellationToken)
    {
        var companyId = _currentUser.CompanyId
            ?? throw new UnauthorizedAccessException(
                "User must belong to a company to create a ticket.");


        var ticket = Ticket.Create(
            companyId,
            _currentUser.UserId,
            request.Subject,
            request.Description);


        _db.Tickets.Add(ticket);

        await _db.SaveChangesAsync(cancellationToken);

        return ticket.Id;
    }
}