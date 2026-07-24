using MediatR;
using Shared.Caching;
using Shared.Domain.Tickets.Events;

namespace Infrastructure.Events.Handlers;

public sealed class TicketCommentAddedEventHandler
    : INotificationHandler<TicketCommentAddedDomainEvent>
{
    private readonly ICacheService _cache;

    public TicketCommentAddedEventHandler(
        ICacheService cache)
    {
        _cache = cache;
    }


    public async Task Handle(
        TicketCommentAddedDomainEvent notification,
        CancellationToken cancellationToken)
    {
        await _cache.IncrementVersionAsync(
            $"tickets:company:{notification.CompanyId}:version");


        await _cache.IncrementVersionAsync(
            $"tickets:company:{notification.CompanyId}:ticket:{notification.TicketId}:version");


        await _cache.IncrementVersionAsync(
            $"tickets:comments:{notification.TicketId}:version");

    }
}