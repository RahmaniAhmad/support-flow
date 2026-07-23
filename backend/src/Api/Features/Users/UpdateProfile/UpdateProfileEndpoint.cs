using MediatR;

namespace Api.Features.Users.UpdateProfile;

public static class UpdateProfileEndpoint
{
    public static IEndpointRouteBuilder MapUpdateProfile(
        this IEndpointRouteBuilder app)
    {
        app.MapPut(
            "/users/profile",
            async (
                UpdateProfileRequest request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                await sender.Send(
                    new UpdateProfileCommand(
                        request.FirstName,
                        request.LastName,
                        request.Phone),
                    cancellationToken);

                return Results.NoContent();
            })
            .RequireAuthorization();

        return app;
    }
}