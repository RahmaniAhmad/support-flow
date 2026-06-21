using Api.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace Api.DependencyInjection;

public static class AuthorizationExtensions
{
    public static IServiceCollection AddApplicationAuthorization(
        this IServiceCollection services)
    {
        services.AddSingleton<IAuthorizationHandler, PermissionHandler>();
        services.AddScoped<ITicketAccessService, TicketAccessService>();

        services.AddAuthorization();

        return services;

    }
}