using Api.Features.Notifications.SignalR;
using Api.Notifications;
using FluentValidation;
using Infrastructure.Pipeline;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Shared.Notifications;

namespace Api.DependencyInjection;

public static class ApiDependencyInjection
{
    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
        });
        services.AddValidatorsFromAssembly(
                  typeof(Program).Assembly);

        services.AddScoped(
                  typeof(IPipelineBehavior<,>),
                  typeof(ValidationBehavior<,>));

        services.AddCacheKeyProviders();
        services.AddApplicationAuthorization();

        services.AddSignalR();

        services.AddSingleton<IUserIdProvider, NotificationUserIdProvider>();
        services.AddScoped<INotificationService, SignalRNotificationService>();

        return services;
    }
}