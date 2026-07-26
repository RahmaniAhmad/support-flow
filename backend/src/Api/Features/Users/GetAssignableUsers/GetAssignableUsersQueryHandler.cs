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
        if (_currentUser.CompanyId is null)
            return [];

        return await _db.Users
            .Where(x =>
                x.CompanyId == _currentUser.CompanyId &&
                x.IsActive &&
                (
                    x.Role == UserRole.Admin ||
                    x.Role == UserRole.Agent
                ))
            .OrderBy(x => x.FirstName)
            .ThenBy(x => x.LastName)
            .Select(x => new GetAssignableUsersResponse(
                x.Id,
                $"{x.FirstName} {x.LastName}".Trim()))
            .ToListAsync(cancellationToken);
    }
}