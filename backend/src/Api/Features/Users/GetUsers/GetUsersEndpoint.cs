using Api.Authorization;
using Api.Filters;
using MediatR;
using Shared.Domain.Users;

namespace Api.Features.Users.GetUsers;


public static class GetUsersEndpoint
{
    public static IEndpointRouteBuilder MapGetUsers(
        this IEndpointRouteBuilder app)
    {

        app.MapGet(
            "/users",
            async (
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var result =
                    await sender.Send(
                        new GetUsersQuery(),
                        cancellationToken);

                return Results.Ok(result);
            })
            .RequireAuthorization()
            .RequirePermission(Permissions.UsersView);


        return app;
    }
}