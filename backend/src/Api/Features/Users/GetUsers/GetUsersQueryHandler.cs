using Api.Authorization;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Users.GetUsers;

public sealed class GetUsersQueryHandler
    : IRequestHandler<GetUsersQuery,
        IReadOnlyList<GetUsersResponse>>
{
    private readonly SupportFlowDbContext _db;
    private readonly IUserAccessService _accessService;


    public GetUsersQueryHandler(
        SupportFlowDbContext db,
        IUserAccessService accessService)
    {
        _db = db;
        _accessService = accessService;
    }


    public async Task<IReadOnlyList<GetUsersResponse>> Handle(
        GetUsersQuery request,
        CancellationToken cancellationToken)
    {
        var users = await _accessService
           .ApplyUserAccessFilter(_db.Users)
           .Select(x => new GetUsersResponse(
               x.Id,
               x.Email,
               x.FirstName,
               x.LastName,
               x.Phone,
               x.Role.ToString(),
               x.IsActive,
               x.CreatedAtUtc))
           .ToListAsync(cancellationToken);


        return users;
    }
}