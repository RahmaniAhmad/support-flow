using Infrastructure.Events.Handlers;
using Shared.Caching;
using Shared.Domain.Tickets.Events;

namespace UnitTests.Tickets;

public sealed class TicketCategoryCacheInvalidationTests
{
    [Fact]
    public async Task Category_changes_invalidate_category_and_ticket_query_groups()
    {
        var cache = new RecordingCacheService();
        var handler = new TicketCategoryUpdatedEventHandler(cache);

        await handler.Handle(
            new TicketCategoryUpdatedDomainEvent(
                Guid.NewGuid(),
                Guid.NewGuid()),
            CancellationToken.None);

        Assert.Contains(
            cache.IncrementedKeys,
            key => key.StartsWith(
                "ticket-categories:company:",
                StringComparison.Ordinal));
        Assert.Contains(
            cache.IncrementedKeys,
            key => key.StartsWith(
                "tickets:company:",
                StringComparison.Ordinal));
    }

    [Fact]
    public async Task Ticket_category_changes_invalidate_ticket_list_and_detail_caches()
    {
        var cache = new RecordingCacheService();
        var handler = new TicketCategoryChangedEventHandler(cache);
        var ticketId = Guid.NewGuid();
        var companyId = Guid.NewGuid();

        await handler.Handle(
            new TicketCategoryChangedDomainEvent(ticketId, companyId),
            CancellationToken.None);

        Assert.Contains(
            $"tickets:company:{companyId}:version",
            cache.IncrementedKeys);
        Assert.Contains(
            $"tickets:company:{companyId}:ticket:{ticketId}:version",
            cache.IncrementedKeys);
    }

    private sealed class RecordingCacheService : ICacheService
    {
        public List<string> IncrementedKeys { get; } = [];

        public Task<T?> GetAsync<T>(string key)
        {
            return Task.FromResult<T?>(default);
        }

        public Task SetAsync<T>(
            string key,
            T value,
            TimeSpan? expiry = null)
        {
            return Task.CompletedTask;
        }

        public Task RemoveAsync(string key)
        {
            return Task.CompletedTask;
        }

        public Task<long> GetVersionAsync(string key)
        {
            return Task.FromResult(1L);
        }

        public Task<long> IncrementVersionAsync(string key)
        {
            IncrementedKeys.Add(key);
            return Task.FromResult(2L);
        }
    }
}
