using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Authentication;

namespace Api.Features.Notifications.Commands.MarkAllAsRead;

public sealed class MarkAllAsReadCommandHandler
    : IRequestHandler<MarkAllAsReadCommand>
{
    private readonly SupportFlowDbContext _db;
    private readonly ICurrentUser _currentUser;


    public MarkAllAsReadCommandHandler(
        SupportFlowDbContext db,
        ICurrentUser currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }


    public async Task Handle(
        MarkAllAsReadCommand request,
        CancellationToken cancellationToken)
    {
        await _db.Notifications
            .Where(x =>
                x.UserId == _currentUser.UserId &&
                x.ReadAtUtc == null)
            .ExecuteUpdateAsync(
                setters =>
                    setters.SetProperty(
                        x => x.ReadAtUtc,
                        DateTime.UtcNow),
                cancellationToken);
    }
}