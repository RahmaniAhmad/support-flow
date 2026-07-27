using Api.Authorization;
using MediatR;
using Shared.Domain.Users;

namespace Api.Features.Users.CreateUser;

public static class CreateUserEndpoint
{
    public static IEndpointRouteBuilder MapCreateUser(
        this IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/users",
            async (
                CreateUserRequest request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(
                    new CreateUserCommand(
                        request.Email,
                        request.Password,
                        request.FirstName,
                        request.LastName,
                        request.Phone,
                        request.Role),
                    cancellationToken);


                return Results.Created(
                    $"/users/{result.Id}",
                    result);
            })
            .RequireAuthorization()
            .RequirePermission(Permissions.UsersCreate);


        return app;
    }
}