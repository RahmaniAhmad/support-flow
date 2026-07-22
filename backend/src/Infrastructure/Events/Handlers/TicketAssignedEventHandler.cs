using MediatR;
using Shared.Caching;
using Shared.Domain.Tickets.Events;

namespace Infrastructure.Events.Handlers;

public sealed class TicketAssignedEventHandler
    : INotificationHandler<TicketAssignedDomainEvent>
{
    private readonly ICacheService _cache;

    public TicketAssignedEventHandler(
        ICacheService cache)
    {
        _cache = cache;
    }


    public async Task Handle(
        TicketAssignedDomainEvent notification,
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