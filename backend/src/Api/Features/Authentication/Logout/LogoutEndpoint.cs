namespace Api.Features.Authentication.Logout;

public static class LogoutEndpoint
{
    public static IEndpointRouteBuilder MapLogout(
        this IEndpointRouteBuilder app)
    {
        app.MapPost("/auth/logout", Logout)
            .WithName("Logout")
            .WithTags("Authentication");

        return app;
    }

    private static IResult Logout(HttpContext context)
    {
        context.Response.Cookies.Delete("access_token");

        context.Response.Cookies.Delete("refresh_token");

        return Results.NoContent();
    }
}