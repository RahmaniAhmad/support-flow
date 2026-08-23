using Microsoft.EntityFrameworkCore;
using Infrastructure.Persistence;
using Shared.Authentication;
using MediatR;
using Infrastructure.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Shared.Domain;
using Shared.Domain.Users;
using Api.Exceptions;
using Api.Errors.ErrorMessages;
using Api.Errors.ErrorCodes;

namespace Api.Features.Authentication.Login;

public sealed class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse?>
{
    private readonly SupportFlowDbContext _db;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;
    private readonly JwtOptions _jwtOptions;
    public LoginCommandHandler(
        SupportFlowDbContext db,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator,
        IRefreshTokenGenerator refreshTokenGenerator,
        IOptions<JwtOptions> jwtOptions)
    {
        _db = db;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
        _refreshTokenGenerator = refreshTokenGenerator;
        _jwtOptions = jwtOptions.Value;
    }

    public async Task<LoginResponse?> Handle(
        LoginCommand command,
        CancellationToken cancellationToken)
    {
        var user = await _db.Users.Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(x => x.Email == command.Email, cancellationToken);

        if (user is null ||
         !_passwordHasher.Verify(
             command.Password,
             user.PasswordHash))
        {
            throw new UnauthorizedException(
                AuthenticationErrorMessages.InvalidCredentials,
                AuthenticationErrorCodes.InvalidCredentials);
        }


        var accessToken = _jwtTokenGenerator.Generate(
                 user.Id,
                 user.CompanyId,
                 user.Email,
                 user.Role);

        var refreshToken = _refreshTokenGenerator.Generate();

        var refreshTokenHash = RefreshTokenHash.Compute(refreshToken);

        var token = user.IssueRefreshToken(
            refreshTokenHash,
            DateTime.UtcNow.Add(_jwtOptions.RefreshTokenLifetime));


        _db.RefreshTokens.Add(token);

        await _db.SaveChangesAsync(cancellationToken);

        return new LoginResponse(
            accessToken,
            refreshToken);
    }
}
