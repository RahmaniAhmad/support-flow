using MediatR;
using Shared.Caching;
using Shared.Domain.Tickets.Events;

namespace Infrastructure.Events.Handlers;

public sealed class TicketPriorityChangedEventHandler
    : INotificationHandler<TicketPriorityChangedDomainEvent>
{
    private readonly ICacheService _cache;

    public TicketPriorityChangedEventHandler(ICacheService cache)
    {
        _cache = cache;
    }

    public async Task Handle(
        TicketPriorityChangedDomainEvent notification,
        CancellationToken cancellationToken)
    {
        await _cache.IncrementVersionAsync(
            $"tickets:company:{notification.CompanyId}:version");

        await _cache.IncrementVersionAsync(
            $"tickets:company:{notification.CompanyId}:ticket:{notification.TicketId}:version");
    }
}
