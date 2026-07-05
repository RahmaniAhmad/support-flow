using Microsoft.EntityFrameworkCore;
using Infrastructure.Persistence;
using Shared.Authentication;
using MediatR;

namespace Api.Features.Authentication.Login;

public sealed class LoginQueryHandler : IRequestHandler<LoginQuery, LoginResponse?>
{
    private readonly SupportFlowDbContext _db;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;

    public LoginQueryHandler(
        SupportFlowDbContext db,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator,
        IRefreshTokenGenerator refreshTokenGenerator)
    {
        _db = db;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
        _refreshTokenGenerator = refreshTokenGenerator;
    }

    public async Task<LoginResponse?> Handle(
        LoginQuery query,
        CancellationToken cancellationToken)
    {
        var user = await _db.Users
            .SingleOrDefaultAsync(x => x.Email == query.Email, cancellationToken);

        if (user is null)
            return null;

        if (!_passwordHasher.Verify(query.Password, user.PasswordHash))
            return null;


        var accessToken = _jwtTokenGenerator.Generate(
                 user.Id,
                 user.CompanyId,
                 user.Email,
                 user.Role);

        var refreshToken = _refreshTokenGenerator.Generate();

        return new LoginResponse(
            accessToken,
            refreshToken);
    }
}
