using Api.Authorization;
using Api.Errors.ErrorCodes;
using Api.Errors.ErrorMessages;
using Api.Exceptions;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Users.ChangeUserStatus;

public sealed class ChangeUserStatusCommandHandler
    : IRequestHandler<ChangeUserStatusCommand>
{
    private readonly SupportFlowDbContext _db;
    private readonly IUserAccessService _accessService;

    public ChangeUserStatusCommandHandler(
        SupportFlowDbContext db,
        IUserAccessService accessService)
    {
        _db = db;
        _accessService = accessService;
    }

    public async Task Handle(
        ChangeUserStatusCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _db.Users
            .FirstOrDefaultAsync(
                x => x.Id == request.UserId,
                cancellationToken);

        if (user is null)
        {
            throw new NotFoundException(
                UserErrorMessages.UserNotFound,
                UserErrorCodes.UserNotFound);
        }

        if (!_accessService.CanChangeStatus(user))
        {
            throw new ForbiddenException(
                UserErrorMessages.CannotChangeStatus,
                UserErrorCodes.CannotChangeStatus);
        }

        if (request.IsActive)
        {
            user.Activate();
        }
        else
        {
            user.Deactivate();
        }

        await _db.SaveChangesAsync(cancellationToken);
    }
}