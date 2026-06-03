using Infrastructure.Persistence;
using Shared.Authentication;
using Shared.Domain;

namespace Api.Features.Authentication.RegisterCompany
{

    public static class RegisterCompanyEndpoint
    {
        public static IEndpointRouteBuilder MapRegisterCompany(
            this IEndpointRouteBuilder app)
        {
            app.MapPost(
                "/auth/register",
                async (
                    RegisterCompanyCommand request,
                    SupportFlowDbContext db,
                    IPasswordHasher passwordHasher,
                    CancellationToken cancellationToken) =>
                {
                    var company = new Company
                    {
                        Id = Guid.NewGuid(),
                        Name = request.CompanyName,
                        CreatedAtUtc = DateTime.UtcNow
                    };

                    var user = new User
                    {
                        Id = Guid.NewGuid(),
                        CompanyId = company.Id,
                        Email = request.Email,
                        PasswordHash = passwordHasher.Hash(request.Password),
                        Role = "Admin",
                        CreatedAtUtc = DateTime.UtcNow
                    };

                    db.Companies.Add(company);
                    db.Users.Add(user);

                    await db.SaveChangesAsync(cancellationToken);

                    return Results.Ok(new
                    {
                        companyId = company.Id,
                        userId = user.Id
                    });

                });

            return app;
        }
    }
}