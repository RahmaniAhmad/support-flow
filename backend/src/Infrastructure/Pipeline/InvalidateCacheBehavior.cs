using MediatR;
using Shared.Caching;

namespace Infrastructure.Pipeline;

public sealed class InvalidateCacheBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IInvalidateCacheCommand
{
    private readonly ICacheService _cache;

    public InvalidateCacheBehavior(ICacheService cache)
    {
        _cache = cache;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var response = await next();

        foreach (var groupKey in request.CacheGroupKeys)
        {
            var versionKey = $"{groupKey}:version";
            await _cache.IncrementVersionAsync(versionKey);
        }

        return response;
    }
}
