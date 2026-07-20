using Api.Extensions;
using Infrastructure.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Api.Features.Authentication.Csrf;

public static class CsrfEndpoint
{
    public static IEndpointRouteBuilder MapCsrf(
        this IEndpointRouteBuilder app)
    {
        app.MapGet("/auth/csrf", GetCsrf)
            .RequireAuthorization()
            .WithTags("Authentication")
            .WithName("GetCsrf");

        return app;
    }

    private static IResult GetCsrf(
        HttpContext context,
        IOptions<JwtOptions> jwtOptions)
    {
        context.IssueXsrfToken(jwtOptions.Value.AccessTokenLifetime);

        return Results.NoContent();
    }
}