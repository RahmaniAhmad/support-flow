using MediatR;
using Shared.Caching;
using Shared.Domain.Tickets.Events;

namespace Infrastructure.Events.Handlers;

public sealed class TicketResolvedEventHandler
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
            $"tickets:company:{notification.CompanyId}:version");

        await _cache.IncrementVersionAsync(
            $"tickets:company:{notification.CompanyId}:ticket:{notification.TicketId}:version");

        await _cache.IncrementVersionAsync(
            $"dashboard:statistics:{notification.CompanyId}:version");

        await _cache.IncrementVersionAsync(
            $"dashboard:agents:{notification.CompanyId}:version");

    }
}