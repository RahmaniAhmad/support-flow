using Microsoft.Extensions.DependencyInjection;
using Shared.Authentication;

namespace Infrastructure.Authentication
{

    public static class DependencyInjection
    {
        public static IServiceCollection AddAuthenticationServices(
            this IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.AddScoped<
                IPasswordHasher,
                PasswordHasher>();

            services.AddScoped<
                IJwtTokenGenerator,
                JwtTokenGenerator>();

            services.AddScoped<
            ICurrentUser,
            CurrentUser>();

            return services;
        }
    }
}