using MediatR;
using Shared.Caching;
using Shared.Domain.Tickets.Events;

namespace Infrastructure.Events.Handlers;

public sealed class TicketInProgressEventHandler
    : INotificationHandler<TicketProgressStartedDomainEvent>
{
    private readonly ICacheService _cache;

    public TicketInProgressEventHandler(
        ICacheService cache)
    {
        _cache = cache;
    }

    public async Task Handle(
        TicketProgressStartedDomainEvent notification,
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