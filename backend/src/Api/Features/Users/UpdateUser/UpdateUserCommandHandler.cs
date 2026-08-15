using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Users.UpdateUser;

public sealed class UpdateUserCommandHandler
    : IRequestHandler<UpdateUserCommand>
{
    private readonly SupportFlowDbContext _db;

    public UpdateUserCommandHandler(
        SupportFlowDbContext db)
    {
        _db = db;
    }

    public async Task Handle(
        UpdateUserCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _db.Users
            .FirstOrDefaultAsync(
                x => x.Id == request.UserId,
                cancellationToken);

        if (user is null)
        {
            throw new InvalidOperationException(
                "User not found.");
        }

        user.UpdateProfile(
            request.FirstName,
            request.LastName,
            request.Phone);

        await _db.SaveChangesAsync(
            cancellationToken);
    }
}