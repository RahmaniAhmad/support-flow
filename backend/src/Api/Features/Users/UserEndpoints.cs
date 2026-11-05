
using Api.Features.Users.Commands.ChangeUserStatus;
using Api.Features.Users.Commands.CreateUser;
using Api.Features.Users.Commands.ResetUserPassword;
using Api.Features.Users.Commands.UpdateProfile;
using Api.Features.Users.Commands.UpdateUser;
using Api.Features.Users.Queries.GetAssignableUsers;
using Api.Features.Users.Queries.GetProfile;
using Api.Features.Users.Queries.GetUser;
using Api.Features.Users.Queries.GetUsers;

namespace Api.Features.Users;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGetProfile();
        endpoints.MapUpdateProfile();
        endpoints.MapGetAssignableUsers();
        endpoints.MapGetUsers();
        endpoints.MapChangeUserStatus();
        endpoints.MapGetUser();
        endpoints.MapCreateUser();
        endpoints.MapUpdateUser();
        endpoints.MapResetUserPassword();

        return endpoints;
    }
}
