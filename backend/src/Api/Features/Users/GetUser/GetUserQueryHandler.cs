using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Users.GetUser;

public sealed class GetUserQueryHandler
    : IRequestHandler<GetUserQuery, GetUserResponse>
{
    private readonly SupportFlowDbContext _db;

    public GetUserQueryHandler(
        SupportFlowDbContext db)
    {
        _db = db;
    }


    public async Task<GetUserResponse> Handle(
        GetUserQuery request,
        CancellationToken cancellationToken)
    {
        var user = await _db.Users
            .AsNoTracking()
            .Where(x => x.Id == request.UserId)
            .Select(x => new GetUserResponse(
                x.Id,
                x.Email,
                x.FirstName,
                x.LastName,
                x.Phone,
                x.Role.ToString(),
                x.IsActive,
                _db.Companies
                .Where(c => c.Id == x.CompanyId)
                .Select(c => c.Name)
                .First(),
                x.CreatedAtUtc))
            .FirstOrDefaultAsync(cancellationToken);


        if (user is null)
        {
            throw new InvalidOperationException(
                "User not found.");
        }


        return user;
    }
}