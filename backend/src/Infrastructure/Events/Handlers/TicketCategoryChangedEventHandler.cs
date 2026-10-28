using MediatR;
using Shared.Caching;
using Shared.Domain.Tickets.Events;

namespace Infrastructure.Events.Handlers;

public sealed class TicketCategoryChangedEventHandler
    : INotificationHandler<TicketCategoryChangedDomainEvent>
{
    private readonly ICacheService _cache;

    public TicketCategoryChangedEventHandler(ICacheService cache)
    {
        _cache = cache;
    }

    public async Task Handle(
        TicketCategoryChangedDomainEvent notification,
        CancellationToken cancellationToken)
    {
        await _cache.IncrementVersionAsync(
            $"tickets:company:{notification.CompanyId}:version");

        await _cache.IncrementVersionAsync(
            $"tickets:company:{notification.CompanyId}:ticket:{notification.TicketId}:version");
    }
}
