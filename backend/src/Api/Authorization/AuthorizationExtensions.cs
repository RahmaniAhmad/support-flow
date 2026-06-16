using Shared.Domain.Users;

namespace Api.Authorization;

public static class AuthorizationExtensions
{
    public static IServiceCollection AddApplicationAuthorization(
        this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(
                Policies.CanManageTickets,
                policy => policy.RequireRole(
                    UserRole.Admin.ToString(),
                    UserRole.Agent.ToString()));
        });

        return services;
    }
}