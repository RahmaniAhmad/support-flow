using Infrastructure.AI;
using Infrastructure.Authentication;
using Infrastructure.Caching;
using Infrastructure.Configuration;
using Infrastructure.Domain;
using Infrastructure.Persistence.Seeders;
using Infrastructure.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Contracts.Storage;

namespace Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<SuperAdminOptions>(
            configuration.GetSection(SuperAdminOptions.SectionName));

        services.Configure<FrontendOptions>(
                 configuration.GetSection(FrontendOptions.SectionName));

        services.AddAuthentication(configuration);
        services.AddCaching(configuration);
        services.AddDomain();

        services.AddAI(configuration);
        services.AddSingleton<IAttachmentStorage, AzureBlobAttachmentStorage>();

        services.AddMediatR(cfg =>
             {
                 cfg.RegisterServicesFromAssembly(
                     typeof(DependencyInjection).Assembly);
             });

        services.AddScoped<SuperAdminSeeder>();

        return services;
    }
}