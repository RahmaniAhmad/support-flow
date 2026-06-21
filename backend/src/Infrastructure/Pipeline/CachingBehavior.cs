using MediatR;
using Shared.Caching;

namespace Infrastructure.Pipeline;

public sealed class CachingBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ICacheService _cache;
    private readonly IServiceProvider _provider;

    public CachingBehavior(
        ICacheService cache,
        IServiceProvider provider)
    {
        _cache = cache;
        _provider = provider;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var keyProvider = GetKeyProvider(request);

        if (keyProvider is null)
            return await next();

        var baseKey = keyProvider.GetBaseKey(request);

        var versionKey = $"{baseKey}:version";
        var version = await _cache.GetVersionAsync(versionKey);

        var cacheKey = $"{baseKey}:v{version}";

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

    private ICacheKeyProvider<TRequest>? GetKeyProvider(TRequest request)
    {
        return _provider.GetService(typeof(ICacheKeyProvider<TRequest>))
            as ICacheKeyProvider<TRequest>;
    }
}