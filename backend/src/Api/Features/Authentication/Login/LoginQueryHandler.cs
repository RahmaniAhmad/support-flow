using Microsoft.EntityFrameworkCore;
using Infrastructure.Persistence;
using Shared.Authentication;

namespace Api.Features.Authentication.Login;

public sealed class LoginQueryHandler
{
    private readonly SupportFlowDbContext _db;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public LoginQueryHandler(
        SupportFlowDbContext db,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _db = db;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<LoginResponse?> Handle(
        LoginQuery query,
        CancellationToken cancellationToken)
    {
        var user = await _db.Users
            .SingleOrDefaultAsync(x => x.Email == query.Email, cancellationToken);

        if (user is null)
            return null;

        var isPasswordValid = _passwordHasher.Verify(query.Password, user.PasswordHash);

        if (!isPasswordValid)
            return null;

        var token = _jwtTokenGenerator.Generate(
            user.Id,
            user.CompanyId,
            user.Email,
            user.Role);

        return new LoginResponse(token);
    }
}
