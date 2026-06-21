using Shared.Caching;

namespace Api.DependencyInjection;

public static class CacheKeyProviderDependencyInjection
{
    public static IServiceCollection AddCacheKeyProviders(
        this IServiceCollection services)
    {
        services.Scan(scan => scan
        .FromApplicationDependencies()
        .AddClasses(classes => classes.AssignableTo(typeof(ICacheKeyProvider<>)))
        .AsImplementedInterfaces()
        .WithScopedLifetime());

        return services;
    }
}