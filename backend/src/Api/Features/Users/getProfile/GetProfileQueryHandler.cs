using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Authentication;

namespace Api.Features.Users.GetProfile;

public sealed class GetProfileQueryHandler
    : IRequestHandler<GetProfileQuery, GetProfileResponse?>
{
    private readonly SupportFlowDbContext _db;
    private readonly ICurrentUser _currentUser;


    public GetProfileQueryHandler(
        SupportFlowDbContext db,
        ICurrentUser currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }


    public async Task<GetProfileResponse?> Handle(
        GetProfileQuery request,
        CancellationToken cancellationToken)
    {
        var profile = await _db.Users
            .AsNoTracking()
            .Where(x => x.Id == _currentUser.UserId)
            .Select(x => new GetProfileResponse(
                x.Email,
                x.FirstName,
                x.LastName,
                x.Phone,
                x.Role,

                x.CompanyId != null
                    ? _db.Companies
                        .Where(c => c.Id == x.CompanyId)
                        .Select(c => c.Name)
                        .FirstOrDefault()
                    : null
            ))
            .FirstOrDefaultAsync(cancellationToken);


        if (profile is null)
            throw new InvalidOperationException("User not found.");


        return profile;
    }
}