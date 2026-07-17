using Api.Options;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.Extensions.Options;

namespace Api.Filters;

public sealed class SecurityFilter : IEndpointFilter
{
    private readonly CorsOptions _corsOptions;

    public SecurityFilter(IOptions<CorsOptions> corsOptions)
    {
        _corsOptions = corsOptions.Value;
    }

    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        var httpContext = context.HttpContext;

        // Skip safe methods
        if (HttpMethods.IsGet(httpContext.Request.Method) ||
            HttpMethods.IsHead(httpContext.Request.Method) ||
            HttpMethods.IsOptions(httpContext.Request.Method))
        {
            return await next(context);
        }

        // Validate Origin
        var origin = httpContext.Request.Headers.Origin.ToString();

        if (!string.IsNullOrEmpty(origin) &&
            !_corsOptions.AllowedOrigins.Contains(origin, StringComparer.OrdinalIgnoreCase))
        {
            return Results.Forbid();
        }

        // Validate CSRF
        var antiforgery = httpContext.RequestServices
            .GetRequiredService<IAntiforgery>();

        await antiforgery.ValidateRequestAsync(httpContext);

        return await next(context);
    }
}