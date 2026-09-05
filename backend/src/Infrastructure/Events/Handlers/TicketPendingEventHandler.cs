using MediatR;
using Shared.Caching;
using Shared.Domain.Tickets.Events;

namespace Infrastructure.Events.Handlers;

public sealed class TicketPendingEventHandler
    : INotificationHandler<TicketPendingDomainEvent>
{
    private readonly ICacheService _cache;

    public TicketPendingEventHandler(ICacheService cache)
    {
        _cache = cache;
    }

    public async Task Handle(
        TicketPendingDomainEvent notification,
        CancellationToken cancellationToken)
    {
        await _cache.IncrementVersionAsync(
            $"tickets:company:{notification.CompanyId}:version");

        await _cache.IncrementVersionAsync(
            $"tickets:company:{notification.CompanyId}:ticket:{notification.TicketId}:version");

        await _cache.IncrementVersionAsync(
            $"dashboard:statistics:{notification.CompanyId}:version");
    }
}