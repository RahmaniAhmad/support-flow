using System.Text.Json;
using Shared.Caching;
using StackExchange.Redis;

namespace Infrastructure.Caching;

public sealed class RedisCacheService : ICacheService
{
    private readonly IDatabase _db;

    public RedisCacheService(IConnectionMultiplexer redis)
    {
        _db = redis.GetDatabase();
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var value = await _db.StringGetAsync(key);

        if (!value.HasValue)
            return default;

        return JsonSerializer.Deserialize<T>(value!);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
    {
        var json = JsonSerializer.Serialize(value);

        await _db.StringSetAsync(
            key,
            json,
            expiry ?? TimeSpan.FromMinutes(5));
    }

    public async Task RemoveAsync(string key)
    {
        await _db.KeyDeleteAsync(key);
    }

    public async Task<long> GetVersionAsync(string key)
    {
        var value = await _db.StringGetAsync(key);

        if (!value.HasValue)
            return 1;

        return (long)value;
    }

    public async Task<long> IncrementVersionAsync(string key)
    {
        return await _db.StringIncrementAsync(key);
    }
}