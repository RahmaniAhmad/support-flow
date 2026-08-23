using Api.Errors.ErrorCodes;
using Api.Errors.ErrorMessages;
using Api.Exceptions;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Authentication;

namespace Api.Features.Authentication.ResetPassword;

public sealed class ResetPasswordCommandHandler
    : IRequestHandler<ResetPasswordCommand>
{
    private readonly SupportFlowDbContext _db;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IPasswordResetTokenGenerator _tokenGenerator;

    public ResetPasswordCommandHandler(
        SupportFlowDbContext db,
        IPasswordHasher passwordHasher,
        IPasswordResetTokenGenerator tokenGenerator)
    {
        _db = db;
        _passwordHasher = passwordHasher;
        _tokenGenerator = tokenGenerator;
    }

    public async Task Handle(
        ResetPasswordCommand request,
        CancellationToken cancellationToken)
    {
        var tokenHash = _tokenGenerator.Hash(
            request.Token);

        var resetToken = await _db.PasswordResetTokens
            .FirstOrDefaultAsync(
                x => x.TokenHash == tokenHash,
                cancellationToken);

        if (resetToken is null ||
                 !resetToken.IsValid(DateTime.UtcNow))
        {
            throw new BadRequestException(
                AuthenticationErrorMessages.InvalidPasswordResetToken,
                AuthenticationErrorCodes.InvalidPasswordResetToken);
        }

        var user = await _db.Users
            .FirstOrDefaultAsync(
                x => x.Id == resetToken.UserId,
                cancellationToken);

        if (user is null || !user.IsActive)
        {
            throw new BadRequestException(
                AuthenticationErrorMessages.PasswordResetUnavailable,
                AuthenticationErrorCodes.PasswordResetUnavailable);
        }

        var passwordHash = _passwordHasher.Hash(
            request.Password);

        user.ChangePassword(passwordHash);

        resetToken.MarkAsUsed();

        await _db.SaveChangesAsync(cancellationToken);
    }
}