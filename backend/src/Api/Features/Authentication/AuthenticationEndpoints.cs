
using Api.Features.Authentication.Csrf;
using Api.Features.Authentication.ForgotPassword;
using Api.Features.Authentication.Login;
using Api.Features.Authentication.Logout;
using Api.Features.Authentication.Me;
using Api.Features.Authentication.Refresh;
using Api.Features.Authentication.RegisterCompany;
using Api.Features.Authentication.ResetPassword;

namespace Api.Features.Authentication;

public static class AuthenticationEndpoints
{
    public static IEndpointRouteBuilder MapAuthenticationEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapRegisterCompany();
        endpoints.MapLogin();
        endpoints.MapLogout();
        endpoints.MapForgotPassword();
        endpoints.MapResetPassword();
        endpoints.MapRefresh();
        endpoints.MapCsrf();
        endpoints.MapMe();

        return endpoints;
    }
}



