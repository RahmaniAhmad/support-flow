using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Authentication;

namespace Api.Features.Users.UpdateProfile;

public sealed class UpdateProfileCommandHandler
    : IRequestHandler<UpdateProfileCommand>
{
    private readonly SupportFlowDbContext _db;
    private readonly ICurrentUser _currentUser;


    public UpdateProfileCommandHandler(
        SupportFlowDbContext db,
        ICurrentUser currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }


    public async Task Handle(
        UpdateProfileCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _db.Users
            .FirstOrDefaultAsync(
                x => x.Id == _currentUser.UserId,
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


        await _db.SaveChangesAsync(cancellationToken);
    }
}