using MediatR;

namespace Api.Features.Users.Queries.GetProfile;

public static class GetProfileEndpoint
{
    public static IEndpointRouteBuilder MapGetProfile(
        this IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/users/profile",
            async (
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(
                    new GetProfileQuery(),
                    cancellationToken);

                return Results.Ok(result);
            })
            .RequireAuthorization();

        return app;
    }
}