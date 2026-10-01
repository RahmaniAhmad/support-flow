using Api.Features.Users.Commands.ChangeUserStatus;
using Api.Features.Users.Commands.CreateUser;
using Api.Features.Users.Queries.GetProfile;
using Api.Features.Users.Queries.GetUser;
using Api.Features.Users.Queries.GetUsers;
using Api.Features.Users.Commands.ResetUserPassword;
using Api.Features.Users.Commands.UpdateProfile;
using Api.Features.Users.Commands.UpdateUser;
using Api.Features.Users.Queries.GetAssignableUsers;

namespace Api.Extensions;


public static class UserEndpointExtensions
{
    public static WebApplication MapUserEndpoints(
        this WebApplication app)
    {
        app.MapGetProfile();
        app.MapUpdateProfile();
        app.MapGetAssignableUsers();
        app.MapGetUsers();
        app.MapChangeUserStatus();
        app.MapGetUser();
        app.MapCreateUser();
        app.MapUpdateUser();
        app.MapResetUserPassword();
        return app;
    }
}
