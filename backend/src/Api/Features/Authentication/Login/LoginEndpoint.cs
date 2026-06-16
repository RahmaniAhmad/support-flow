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
        LoginQuery query,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(query, cancellationToken);

        if (result is null)
            return Results.Unauthorized();

        return Results.Ok(result);
    }
}
