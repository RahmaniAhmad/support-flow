using MediatR;
using Shared.Caching;
using Shared.Domain.Tickets.Events;

namespace Infrastructure.Events.Handlers;

public sealed class TicketInProgressEventHandler
    : INotificationHandler<TicketInProgressDomainEvent>
{
    private readonly ICacheService _cache;

    public TicketInProgressEventHandler(
        ICacheService cache)
    {
        _cache = cache;
    }

    public async Task Handle(
        TicketInProgressDomainEvent notification,
        CancellationToken cancellationToken)
    {
        await _cache.IncrementVersionAsync(
            $"tickets:company:{notification.CompanyId}:version");

        await _cache.IncrementVersionAsync(
            $"tickets:company:{notification.CompanyId}:ticket:{notification.TicketId}:version");

        await _cache.IncrementVersionAsync(
            $"dashboard:{notification.CompanyId}:version");
    }
}