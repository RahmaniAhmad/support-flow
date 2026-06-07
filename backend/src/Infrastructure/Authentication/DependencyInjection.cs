using Infrastructure.Notifications;
using Microsoft.Extensions.DependencyInjection;
using Shared.Authentication;
using Shared.Notifications;

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

            services.AddScoped<
            INotificationService,
            LoggingNotificationService>();

            return services;
        }
    }
}