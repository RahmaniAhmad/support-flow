using Api.Authorization;
using Api.DependencyInjection;
using FluentValidation;
using Infrastructure.Pipeline;
using MediatR;

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

        return services;
    }
}