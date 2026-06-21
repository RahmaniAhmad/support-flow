using Infrastructure.Authentication;
using Infrastructure.Caching;
using Infrastructure.Domain;
using Infrastructure.Notifications;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAuthentication(configuration);
        services.AddCaching(configuration);
        services.AddDomain();
        services.AddNotifications();

        return services;
    }
}