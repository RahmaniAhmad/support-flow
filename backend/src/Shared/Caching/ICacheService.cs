namespace Shared.Caching;

public interface ICacheService
{
    Task<T?> GetAsync<T>(string key);
    Task SetAsync<T>(string key, T value, TimeSpan? expiry = null);
    Task RemoveAsync(string key);
    Task<long> GetVersionAsync(string key);
    Task<long> IncrementVersionAsync(string key);
}