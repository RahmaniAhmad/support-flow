using Api.Extensions;
using Infrastructure.Authentication;
using MediatR;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.Extensions.Options;

namespace Api.Features.Authentication.Login;

public static class LoginEndpoint
{
    public static IEndpointRouteBuilder MapLogin(
        this IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/auth/login",
            LoginAsync)
            .WithName("Login")
            .WithTags("Authentication");

        return app;
    }

    private static async Task<IResult> LoginAsync(
        HttpContext context,
        LoginCommand command,
        ISender sender,
        IOptions<JwtOptions> jwtOptions,
        CancellationToken cancellationToken)
    {
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

        context.IssueXsrfToken(jwtOptions.Value.AccessTokenLifetime);

        return Results.NoContent();
    }
}
