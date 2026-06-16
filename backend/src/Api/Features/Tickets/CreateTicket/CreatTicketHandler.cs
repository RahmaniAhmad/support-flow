
using Api.Authorization;
using Infrastructure.Persistence;
using MediatR;
using Shared.Authentication;
using Shared.Domain.Tickets;

namespace Api.Features.Tickets.CreateTicket;

public class CreateTicketCommandHandler : IRequestHandler<CreateTicketCommand, Guid>
{
    private readonly SupportFlowDbContext _db;
    private readonly ICurrentUser _currentUser;
    private readonly ITicketAccessService _accessService;

    public CreateTicketCommandHandler(SupportFlowDbContext db, ICurrentUser currentUser, ITicketAccessService accessService)
    {
        _db = db;
        _currentUser = currentUser;
        _accessService = accessService;
    }
    public async Task<Guid> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
    {
        var ticket = Ticket.Create(request.CompanyId, request.UserId, request.Subject, request.Description);

        if (!_accessService.CanAccessTicket(_currentUser.UserId, ticket, _currentUser.Role))
            throw new UnauthorizedAccessException();

        _db.Tickets.Add(ticket);
        await _db.SaveChangesAsync(cancellationToken);

        return ticket.Id;
    }
}
