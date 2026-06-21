using Infrastructure.Pipeline;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Caching;

namespace Infrastructure.Caching;

public static class CachingDependencyInjection
{
    public static IServiceCollection AddCaching(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddRedis(configuration);

        services.AddScoped<ICacheService, RedisCacheService>();

        services.AddScoped(
            typeof(IPipelineBehavior<,>),
            typeof(CachingBehavior<,>));

        return services;
    }
}