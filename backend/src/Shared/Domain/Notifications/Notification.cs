using Shared.Domain.Base;

namespace Shared.Domain.Notifications;

public sealed class Notification : Entity
{
    public Guid UserId { get; private set; }

    public Guid CompanyId { get; private set; }

    public NotificationType Type { get; private set; }

    public string Title { get; private set; } = string.Empty;

    public string Message { get; private set; } = string.Empty;

    public Guid TicketId { get; private set; }

    public DateTime CreatedAtUtc { get; private set; }

    public DateTime? ReadAtUtc { get; private set; }

    private Notification()
    {
    }

    public static Notification Create(
        Guid userId,
        Guid companyId,
        Guid ticketId,
        NotificationType type,
        string title,
        string message)
    {
        ArgumentOutOfRangeException.ThrowIfEqual(
            userId,
            Guid.Empty);

        ArgumentOutOfRangeException.ThrowIfEqual(
            companyId,
            Guid.Empty);

        ArgumentOutOfRangeException.ThrowIfEqual(
            ticketId,
            Guid.Empty);

        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        ArgumentException.ThrowIfNullOrWhiteSpace(message);

        return new Notification
        {
            UserId = userId,
            CompanyId = companyId,
            TicketId = ticketId,
            Type = type,
            Title = title.Trim(),
            Message = message.Trim(),
            CreatedAtUtc = DateTime.UtcNow
        };
    }

    public void MarkAsRead()
    {
        if (ReadAtUtc is not null)
        {
            return;
        }

        ReadAtUtc = DateTime.UtcNow;
    }
}