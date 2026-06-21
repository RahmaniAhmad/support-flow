namespace Shared.Caching;

public interface ICacheKeyProvider<in TRequest>
{
    string GetBaseKey(TRequest request);
    TimeSpan? Expiration => TimeSpan.FromMinutes(5);
}