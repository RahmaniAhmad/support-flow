using Api.Authorization;
using Api.Errors.ErrorCodes;
using Api.Errors.ErrorMessages;
using Api.Exceptions;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Authentication;

namespace Api.Features.Tickets.AssignTicket;

public sealed class AssignTicketCommandHandler
    : IRequestHandler<AssignTicketCommand>
{
    private readonly SupportFlowDbContext _db;
    private readonly ICurrentUser _currentUser;
    private readonly ITicketAccessService _accessService;

    public AssignTicketCommandHandler(SupportFlowDbContext db, ICurrentUser currentUser, ITicketAccessService accessService)
    {
        _db = db;
        _currentUser = currentUser;
        _accessService = accessService;
    }

    public async Task Handle(
        AssignTicketCommand request,
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
                TicketErrorMessages.TicketNotFound,
                TicketErrorCodes.TicketNotFound);
        }

        var assignedUser = await _db.Users
        .Where(x =>
            x.IsActive &&
            x.CompanyId == ticket.CompanyId)
        .FirstOrDefaultAsync(
            x => x.Id == request.AssignedToUserId,
            cancellationToken);


        if (assignedUser is null)
        {
            throw new NotFoundException(
                TicketErrorMessages.AssignedUserNotFound,
                TicketErrorCodes.AssignedUserNotFound);
        }

        if (!_accessService.CanAssignTicketTo(
              ticket,
              assignedUser))
        {
            throw new ForbiddenException(
                TicketErrorMessages.CannotAssignTicket,
                TicketErrorCodes.CannotAssignTicket);
        }

        ticket.AssignTo(_currentUser.UserId, request.AssignedToUserId);

        await _db.SaveChangesAsync(cancellationToken);
    }
}
