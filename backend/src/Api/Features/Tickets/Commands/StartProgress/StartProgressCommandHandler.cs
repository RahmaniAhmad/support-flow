using Api.Authorization;
using Api.Exceptions;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Authentication;

namespace Api.Features.Tickets.Commands.StartProgress;

public sealed class StartProgressCommandHandler
    : IRequestHandler<StartProgressCommand>
{
    private readonly SupportFlowDbContext _db;
    private readonly ICurrentUser _currentUser;
    private readonly ITicketAccessService _accessService;


    public StartProgressCommandHandler(SupportFlowDbContext db, ICurrentUser currentUser, ITicketAccessService accessService)
    {
        _db = db;
        _currentUser = currentUser;
        _accessService = accessService;
    }

    public async Task Handle(
        StartProgressCommand request,
        CancellationToken cancellationToken)
    {
        var ticket = await _accessService
            .ApplyTicketAccessFilter(_db.Tickets)
            .FirstOrDefaultAsync(
                x => x.Id == request.TicketId,
                cancellationToken);

        if (ticket is null)
        {
            throw new NotFoundException(
                TicketErrors.NotFoundMessage,
                TicketErrors.NotFoundCode);
        }

        if (!_accessService.CanStartProgress(ticket))
        {
            throw new ForbiddenException(
                TicketErrors.CannotStartProgressMessage,
                TicketErrors.CannotStartProgressCode);
        }

        ticket.StartProgress(_currentUser.UserId);

        await _db.SaveChangesAsync(cancellationToken);
    }
}
