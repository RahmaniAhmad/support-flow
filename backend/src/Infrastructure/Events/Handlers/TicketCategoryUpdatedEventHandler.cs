using MediatR;
using Shared.Caching;
using Shared.Domain.Tickets.Events;

namespace Infrastructure.Events.Handlers;

public sealed class TicketCategoryUpdatedEventHandler
    : INotificationHandler<TicketCategoryUpdatedDomainEvent>
{
    private readonly ICacheService _cache;

    public TicketCategoryUpdatedEventHandler(ICacheService cache)
    {
        _cache = cache;
    }

    public async Task Handle(
        TicketCategoryUpdatedDomainEvent notification,
        CancellationToken cancellationToken)
    {
        await _cache.IncrementVersionAsync(
            $"ticket-categories:company:{notification.CompanyId}:version");

        await _cache.IncrementVersionAsync(
            $"tickets:company:{notification.CompanyId}:version");
    }
}
