using Infrastructure.AI;
using Infrastructure.Authentication;
using Infrastructure.Caching;
using Infrastructure.Domain;
using Infrastructure.Notifications;
using Infrastructure.Persistence.Seeders;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<SuperAdminOptions>(
            configuration.GetSection(SuperAdminOptions.SectionName));

        services.AddAuthentication(configuration);
        services.AddCaching(configuration);
        services.AddDomain();
        services.AddNotifications();

        services.AddAI(configuration);

        services.AddMediatR(cfg =>
             {
                 cfg.RegisterServicesFromAssembly(
                     typeof(DependencyInjection).Assembly);
             });

        services.AddScoped<SuperAdminSeeder>();

        return services;
    }
}