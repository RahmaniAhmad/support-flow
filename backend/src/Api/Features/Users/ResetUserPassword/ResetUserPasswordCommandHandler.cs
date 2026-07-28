using Api.Authorization;
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
        if (string.IsNullOrWhiteSpace(request.Password))
        {
            throw new InvalidOperationException(
                "Password is required.");
        }

        if (request.Password.Length < 8)
        {
            throw new InvalidOperationException(
                "Password must be at least 8 characters.");
        }

        var user = await _db.Users
            .FirstOrDefaultAsync(
                x => x.Id == request.UserId,
                cancellationToken);


        if (user is null)
        {
            throw new InvalidOperationException(
                "User not found.");
        }

        if (!_accessService.CanResetPassword(user))
        {
            throw new UnauthorizedAccessException(
                "You cannot reset this user's password.");
        }

        var passwordHash =
            _passwordHasher.Hash(request.Password);


        user.ChangePassword(passwordHash);


        await _db.SaveChangesAsync(
            cancellationToken);
    }
}