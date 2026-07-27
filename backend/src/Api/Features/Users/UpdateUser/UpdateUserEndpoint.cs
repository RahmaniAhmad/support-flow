using Api.Authorization;
using MediatR;
using Shared.Domain.Users;

namespace Api.Features.Users.UpdateUser;

public static class UpdateUserEndpoint
{
    public static IEndpointRouteBuilder MapUpdateUser(
        this IEndpointRouteBuilder app)
    {
        app.MapPut(
            "/users/{id:guid}",
            async (
                Guid id,
                UpdateUserRequest request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                await sender.Send(
                    new UpdateUserCommand(
                        id,
                        request.FirstName,
                        request.LastName,
                        request.Phone),
                    cancellationToken);


                return Results.NoContent();
            })
            .RequireAuthorization()
            .RequirePermission(Permissions.UsersUpdate);


        return app;
    }
}