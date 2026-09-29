using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Authentication;

namespace Api.Features.Notifications.Commands.MarkAsRead;

public sealed class MarkAsReadCommandHandler
    : IRequestHandler<MarkAsReadCommand>
{
    private readonly SupportFlowDbContext _db;
    private readonly ICurrentUser _currentUser;


    public MarkAsReadCommandHandler(
        SupportFlowDbContext db,
        ICurrentUser currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }


    public async Task Handle(
        MarkAsReadCommand request,
        CancellationToken cancellationToken)
    {
        var notification =
            await _db.Notifications
                .FirstOrDefaultAsync(
                    x =>
                        x.Id == request.NotificationId &&
                        x.UserId == _currentUser.UserId,
                    cancellationToken);


        if (notification is null)
        {
            throw new KeyNotFoundException(
                "Notification not found.");
        }


        notification.MarkAsRead();


        await _db.SaveChangesAsync(
            cancellationToken);
    }
}