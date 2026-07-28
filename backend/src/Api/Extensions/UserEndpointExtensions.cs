using Api.Features.Users.ChangeUserStatus;
using Api.Features.Users.CreateUser;
using Api.Features.Users.GetAssignableUsers;
using Api.Features.Users.GetProfile;
using Api.Features.Users.GetUser;
using Api.Features.Users.GetUsers;
using Api.Features.Users.ResetUserPassword;
using Api.Features.Users.UpdateProfile;
using Api.Features.Users.UpdateUser;

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
