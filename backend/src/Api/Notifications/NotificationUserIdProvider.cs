using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Api.Notifications;

public sealed class NotificationUserIdProvider : IUserIdProvider
{
    public string? GetUserId(HubConnectionContext connection)
    {
        return connection.User?.FindFirstValue(
            ClaimTypes.NameIdentifier);
    }
}