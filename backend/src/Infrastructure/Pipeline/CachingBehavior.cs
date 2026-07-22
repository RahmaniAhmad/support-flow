using MediatR;
using Shared.Caching;

namespace Infrastructure.Pipeline;

public sealed class CachingBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ICacheService _cache;
    private readonly IServiceProvider _serviceProvider;

    public CachingBehavior(
        ICacheService cache,
        IServiceProvider serviceProvider)
    {
        _cache = cache;
        _serviceProvider = serviceProvider;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var keyProvider = GetKeyProvider();

        if (keyProvider is null)
            return await next();

        var key = keyProvider.GetKey(request);
        var group = keyProvider.GetGroup(request);
        var version = await _cache.GetVersionAsync(
            $"{group}:version");
        var cacheKey = $"{key}:v{version}";

        var cached = await _cache.GetAsync<TResponse>(cacheKey);
        if (cached is not null)
            return cached;

        var response = await next();

        await _cache.SetAsync(
            cacheKey,
            response,
            keyProvider.Expiration);

        return response;
    }

    private ICacheKeyProvider<TRequest>? GetKeyProvider()
    {
        return _serviceProvider.GetService(typeof(ICacheKeyProvider<TRequest>))
            as ICacheKeyProvider<TRequest>;
    }
}