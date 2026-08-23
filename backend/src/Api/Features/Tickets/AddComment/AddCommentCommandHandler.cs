using Api.Authorization;
using Api.Errors.ErrorCodes;
using Api.Errors.ErrorMessages;
using Api.Exceptions;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Authentication;

namespace Api.Features.Tickets.AddComment;


public sealed class AddCommentCommandHandler
    : IRequestHandler<AddCommentCommand, Guid>
{
    private readonly SupportFlowDbContext _db;
    private readonly ICurrentUser _currentUser;
    private readonly ITicketAccessService _accessService;

    public AddCommentCommandHandler(SupportFlowDbContext db, ICurrentUser currentUser, ITicketAccessService accessService)
    {
        _db = db;
        _currentUser = currentUser;
        _accessService = accessService;
    }

    public async Task<Guid> Handle(
        AddCommentCommand request,
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

        var commentId = ticket.AddComment(
            _currentUser.UserId,
            request.Content);

        await _db.SaveChangesAsync(cancellationToken);

        return commentId;
    }
}
