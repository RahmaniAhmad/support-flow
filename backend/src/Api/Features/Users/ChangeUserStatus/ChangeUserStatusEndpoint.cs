using Api.Authorization;
using MediatR;
using Shared.Domain.Users;

namespace Api.Features.Users.ChangeUserStatus;

public static class ChangeUserStatusEndpoint
{
    public static IEndpointRouteBuilder MapChangeUserStatus(
        this IEndpointRouteBuilder app)
    {
        app.MapPut(
            "/users/{id:guid}/status",
            async (
                Guid id,
                ChangeUserStatusRequest request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                await sender.Send(
                    new ChangeUserStatusCommand(
                        id,
                        request.IsActive),
                    cancellationToken);

                return Results.NoContent();
            })
            .RequireAuthorization()
            .RequirePermission(Permissions.UsersUpdate);

        return app;
    }
}
