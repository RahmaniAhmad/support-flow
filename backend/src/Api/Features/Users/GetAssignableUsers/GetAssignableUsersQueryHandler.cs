using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Authentication;
using Shared.Domain.Users;

namespace Api.Features.Users.GetAssignableUsers;

public sealed class GetAssignableUsersQueryHandler
    : IRequestHandler<GetAssignableUsersQuery, List<GetAssignableUsersResponse>>
{
    private readonly SupportFlowDbContext _db;
    private readonly ICurrentUser _currentUser;

    public GetAssignableUsersQueryHandler(
        SupportFlowDbContext db,
        ICurrentUser currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<List<GetAssignableUsersResponse>> Handle(
        GetAssignableUsersQuery request,
        CancellationToken cancellationToken)
    {
        var query = _db.Users
        .Where(x => x.IsActive);

        query = _currentUser.Role switch
        {
            UserRole.SuperAdmin =>
                query.Where(x =>
                    x.Role == UserRole.Admin ||
                    x.Role == UserRole.Agent),


            UserRole.Admin =>
             query.Where(x =>
                 x.CompanyId == _currentUser.CompanyId
                 &&
                 x.Role == UserRole.Agent),


            UserRole.Agent =>
                query.Where(x =>
                    x.Id == _currentUser.UserId),


            _ =>
                query.Where(x => false)
        };

        return await query
                    .OrderBy(x => x.FirstName)
                    .ThenBy(x => x.LastName)
                    .Select(x => new GetAssignableUsersResponse(
                        x.Id,
                        $"{x.FirstName} {x.LastName}".Trim()))
                    .ToListAsync(cancellationToken);

    }
}