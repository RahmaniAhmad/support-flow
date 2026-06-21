using MediatR;
using Shared.Caching;
using Shared.Domain.Tickets.Events;

namespace Infrastructure.Events.Handlers;

public class TicketResolvedEventHandler
    : INotificationHandler<TicketResolvedDomainEvent>
{
    private readonly ICacheService _cache;

    public TicketResolvedEventHandler(ICacheService cache)
    {
        _cache = cache;
    }

    public async Task Handle(
        TicketResolvedDomainEvent notification,
        CancellationToken cancellationToken)
    {
        await _cache.IncrementVersionAsync(
                $"dashboard:{notification.CompanyId}:version");
    }
}