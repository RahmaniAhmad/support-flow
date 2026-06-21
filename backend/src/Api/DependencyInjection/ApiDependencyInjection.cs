using Api.Authorization;
using Api.DependencyInjection;

namespace Api.DependencyInjection;

public static class ApiDependencyInjection
{
    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
        });

        services.AddCacheKeyProviders();
        services.AddApplicationAuthorization();

        return services;
    }
}