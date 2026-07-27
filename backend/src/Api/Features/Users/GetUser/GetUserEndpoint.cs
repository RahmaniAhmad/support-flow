using Api.Authorization;
using MediatR;
using Shared.Domain.Users;

namespace Api.Features.Users.GetUser;

public static class GetUserEndpoint
{
    public static IEndpointRouteBuilder MapGetUser(
        this IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/users/{id:guid}",
            async (
                Guid id,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(
                    new GetUserQuery(id),
                    cancellationToken);

                return Results.Ok(result);
            })
            .RequireAuthorization()
            .RequirePermission(Permissions.UsersView);


        return app;
    }
}