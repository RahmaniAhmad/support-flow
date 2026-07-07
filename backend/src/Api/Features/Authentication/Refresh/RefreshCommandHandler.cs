using Microsoft.EntityFrameworkCore;
using MediatR;
using Infrastructure.Persistence;
using Shared.Authentication;
using Infrastructure.Authentication;
using Microsoft.Extensions.Options;

namespace Api.Features.Authentication.Refresh;

public sealed class RefreshCommandHandler
    : IRequestHandler<RefreshCommand, RefreshResponse?>
{
    private readonly SupportFlowDbContext _db;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;
    private readonly JwtOptions _jwtOptions;
    public RefreshCommandHandler(
        SupportFlowDbContext db,
        IJwtTokenGenerator jwtTokenGenerator,
        IRefreshTokenGenerator refreshTokenGenerator,
        IOptions<JwtOptions> jwtOptions)
    {
        _db = db;
        _jwtTokenGenerator = jwtTokenGenerator;
        _refreshTokenGenerator = refreshTokenGenerator;
        _jwtOptions = jwtOptions.Value;
    }

    public async Task<RefreshResponse?> Handle(
        RefreshCommand command,
        CancellationToken cancellationToken)
    {
        var tokenHash = RefreshTokenHash.Compute(command.RefreshToken);

        var refreshToken = await _db.RefreshTokens
            .Include(x => x.User)
            .SingleOrDefaultAsync(
                x => x.TokenHash == tokenHash,
                cancellationToken);

        if (refreshToken is null)
            return null;

        if (!refreshToken.IsActive)
            return null;


        var newRefreshToken = _refreshTokenGenerator.Generate();

        var newRefreshTokenHash =
            RefreshTokenHash.Compute(newRefreshToken);

        var token = refreshToken.User.RotateRefreshToken(
            refreshToken,
            newRefreshTokenHash,
            DateTime.UtcNow.Add(_jwtOptions.RefreshTokenLifetime));

        _db.RefreshTokens.Add(token);

        var accessToken =
            _jwtTokenGenerator.Generate(
                refreshToken.User.Id,
                refreshToken.User.CompanyId,
                refreshToken.User.Email,
                refreshToken.User.Role);

        await _db.SaveChangesAsync(cancellationToken);

        return new RefreshResponse(
            accessToken,
            newRefreshToken);
    }
}