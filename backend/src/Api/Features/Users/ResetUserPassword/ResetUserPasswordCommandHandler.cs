using Api.Authorization;
using Api.Errors.ErrorCodes;
using Api.Errors.ErrorMessages;
using Api.Exceptions;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Authentication;

namespace Api.Features.Users.ResetUserPassword;

public sealed class ResetUserPasswordCommandHandler
    : IRequestHandler<ResetUserPasswordCommand>
{
    private readonly SupportFlowDbContext _db;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserAccessService _accessService;

    public ResetUserPasswordCommandHandler(
        SupportFlowDbContext db,
        IPasswordHasher passwordHasher,
        IUserAccessService accessService)
    {
        _db = db;
        _passwordHasher = passwordHasher;
        _accessService = accessService;
    }


    public async Task Handle(
        ResetUserPasswordCommand request,
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

        if (!_accessService.CanResetPassword(user))
        {
            throw new ForbiddenException(
              UserErrorMessages.CannotResetPassword,
              UserErrorCodes.CannotResetPassword);
        }

        var passwordHash =
            _passwordHasher.Hash(request.Password);


        user.ChangePassword(passwordHash);


        await _db.SaveChangesAsync(
            cancellationToken);
    }
}