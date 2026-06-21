namespace Shared.Caching;

public interface IInvalidateCacheCommand
{
    string[] CacheGroupKeys { get; }
}