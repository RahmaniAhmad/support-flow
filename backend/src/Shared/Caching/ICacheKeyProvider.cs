namespace Shared.Caching;

public interface ICacheKeyProvider<in TRequest>
{
    string GetKey(TRequest request);
    string GetGroup(TRequest request)
        => GetKey(request);
    TimeSpan? Expiration => CacheExpiration.Default;
}