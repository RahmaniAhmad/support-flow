using Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Shared.Domain.Base;

namespace Infrastructure.Domain;


public static class DomainDependencyInjection
{
    public static IServiceCollection AddDomain(
        this IServiceCollection services)
    {
        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

        return services;
    }
}