using Api.Features.Users.GetProfile;
using Api.Features.Users.UpdateProfile;

namespace Api.Extensions;


public static class UserEndpointExtensions
{
    public static WebApplication MapUserEndpoints(
        this WebApplication app)
    {
        app.MapGetProfile();
        app.MapUpdateProfile();

        return app;
    }
}
