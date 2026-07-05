using MediatR;

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
        LoginQuery query,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(query, cancellationToken);

        if (result is null)
            return Results.Unauthorized();

        context.Response.Cookies.Append(
            "access_token",
            result.AccessToken,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = context.Request.IsHttps,
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.UtcNow.AddMinutes(15),
                Path = "/"
            });

        context.Response.Cookies.Append(
            "refresh_token",
            result.RefreshToken,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = context.Request.IsHttps,
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.UtcNow.AddDays(30),
                Path = "/auth"
            });

        return Results.NoContent();
    }
}
