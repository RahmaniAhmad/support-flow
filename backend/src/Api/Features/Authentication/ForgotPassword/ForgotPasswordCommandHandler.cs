using Infrastructure.Configuration;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Shared.Authentication;

namespace Api.Features.Authentication.ForgotPassword;

public sealed class ForgotPasswordCommandHandler
    : IRequestHandler<ForgotPasswordCommand>
{
    private readonly SupportFlowDbContext _db;
    private readonly IPasswordResetTokenGenerator _tokenGenerator;
    private readonly FrontendOptions _frontendOptions;
    private readonly ILogger<ForgotPasswordCommandHandler> _logger;

    public ForgotPasswordCommandHandler(
        SupportFlowDbContext db,
        IPasswordResetTokenGenerator tokenGenerator,
        IOptions<FrontendOptions> frontendOptions,
        ILogger<ForgotPasswordCommandHandler> logger)
    {
        _db = db;
        _tokenGenerator = tokenGenerator;
        _frontendOptions = frontendOptions.Value;
        _logger = logger;
    }

    public async Task Handle(
        ForgotPasswordCommand request,
        CancellationToken cancellationToken)
    {
        var email = request.Email.Trim().ToLowerInvariant();

        var user = await _db.Users
            .FirstOrDefaultAsync(
                x => x.Email == email,
                cancellationToken);

        // Do not reveal whether the email exists.
        if (user is null)
            return;

        if (!user.IsActive)
            return;

        // Invalidate existing unused reset tokens.
        var existingTokens = await _db.PasswordResetTokens
            .Where(x =>
                x.UserId == user.Id &&
                x.UsedAtUtc == null &&
                x.ExpiresAtUtc > DateTime.UtcNow)
            .ToListAsync(cancellationToken);

        foreach (var token in existingTokens)
        {
            token.MarkAsUsed();
        }

        var rawToken = _tokenGenerator.Generate();

        var tokenHash = _tokenGenerator.Hash(rawToken);

        var expiresAtUtc = DateTime.UtcNow.AddMinutes(30);

        user.CreatePasswordResetToken(
            tokenHash,
            expiresAtUtc);

        await _db.SaveChangesAsync(cancellationToken);

        var resetUrl =
        $"{_frontendOptions.Url.TrimEnd('/')}/reset-password" +
        $"?token={Uri.EscapeDataString(rawToken)}";

        //TODO: Send the reset URL to the user's email address using an email service.

        _logger.LogInformation(
            "Password reset URL for {Email}: {ResetUrl}",
            user.Email,
            resetUrl);
    }
}