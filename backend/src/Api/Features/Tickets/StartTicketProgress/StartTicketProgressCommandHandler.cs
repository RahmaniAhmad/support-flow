using Api.Authorization;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Authentication;

namespace Api.Features.Tickets.StartTicketProgress;

public sealed class StartTicketProgressCommandHandler
    : IRequestHandler<StartTicketProgressCommand>
{
    private readonly SupportFlowDbContext _db;

    private readonly ICurrentUser _currentUser;
    private readonly ITicketAccessService _accessService;


    public StartTicketProgressCommandHandler(SupportFlowDbContext db, ICurrentUser currentUser, ITicketAccessService accessService)
    {
        _db = db;
        _currentUser = currentUser;
        _accessService = accessService;
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

        if (!_accessService.CanAccessTicket(_currentUser.UserId, ticket, _currentUser.Role))
            throw new UnauthorizedAccessException();

        ticket.StartProgress();

        await _db.SaveChangesAsync(cancellationToken);
    }
}
