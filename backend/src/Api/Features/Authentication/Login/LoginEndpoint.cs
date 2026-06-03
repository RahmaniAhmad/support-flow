using Microsoft.EntityFrameworkCore;
using Infrastructure.Persistence;
using Shared.Authentication;
using Shared.Domain;

namespace Api.Features.Authentication.Login
{

    public static class LoginEndpoint
    {
        public static IEndpointRouteBuilder MapLogin(
            this IEndpointRouteBuilder app)
        {
            app.MapPost(
                "/auth/login",
                LoginAsync)
                .WithName("Login")
                .WithTags("Authentication");

            return app;
        }

        private static async Task<IResult> LoginAsync(
            LoginRequest request,
            SupportFlowDbContext db,
            IPasswordHasher passwordHasher,
            IJwtTokenGenerator jwtTokenGenerator,
            CancellationToken cancellationToken)
        {
            User? user = await db.Users
                .SingleOrDefaultAsync(
                    x => x.Email == request.Email,
                    cancellationToken);

            if (user is null)
            {
                return Results.Unauthorized();
            }

            bool isPasswordValid =
                passwordHasher.Verify(
                    request.Password,
                    user.PasswordHash);

            if (!isPasswordValid)
            {
                return Results.Unauthorized();
            }

            string accessToken =
                jwtTokenGenerator.Generate(
                    user.Id,
                    user.CompanyId,
                    user.Email,
                    user.Role);

            return Results.Ok(
                new LoginResponse(accessToken));
        }
    }
}