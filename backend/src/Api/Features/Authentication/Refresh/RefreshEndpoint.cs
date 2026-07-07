using Infrastructure.Authentication;
using MediatR;
using Microsoft.Extensions.Options;

namespace Api.Features.Authentication.Refresh;

public static class RefreshEndpoint
{
    public static IEndpointRouteBuilder MapRefresh(
        this IEndpointRouteBuilder app)
    {
        app.MapPost("/auth/refresh", RefreshAsync)
            .WithTags("Authentication")
            .WithName("Refresh");

        return app;
    }

    private static async Task<IResult> RefreshAsync(
        HttpContext context,
        ISender sender,
        IOptions<JwtOptions> jwtOptions,
        CancellationToken cancellationToken)
    {
        var refreshToken = context.Request.Cookies["refresh_token"];

        if (string.IsNullOrWhiteSpace(refreshToken))
            return Results.Unauthorized();

        var command = new RefreshCommand(refreshToken);

        var result = await sender.Send(command, cancellationToken);

        if (result is null)
            return Results.Unauthorized();

        context.Response.Cookies.Append(
            "access_token",
            result.AccessToken,
            AuthenticationCookieOptions.AccessToken(context, jwtOptions.Value.AccessTokenLifetime));

        context.Response.Cookies.Append(
            "refresh_token",
            result.RefreshToken,
            AuthenticationCookieOptions.RefreshToken(context, jwtOptions.Value.RefreshTokenLifetime));

        return Results.NoContent();
    }
}