using Infrastructure.Persistence;
using MediatR;
using Shared.Authentication;
using Shared.Domain;

namespace Api.Features.Authentication.RegisterCompany;

public class RegisterCompanyCommandHandler
    : IRequestHandler<RegisterCompanyCommand, RegisterCompanyResponse>
{
    private readonly SupportFlowDbContext _db;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterCompanyCommandHandler(
        SupportFlowDbContext db,
        IPasswordHasher passwordHasher)
    {
        _db = db;
        _passwordHasher = passwordHasher;
    }

    public async Task<RegisterCompanyResponse> Handle(
        RegisterCompanyCommand request,
        CancellationToken cancellationToken)
    {
        var company = new Company
        {
            Name = request.CompanyName,
            CreatedAtUtc = DateTime.UtcNow
        };

        var user = new User
        {
            CompanyId = company.Id,
            Email = request.Email,
            PasswordHash = _passwordHasher.Hash(request.Password),
            Role = "Admin",
            CreatedAtUtc = DateTime.UtcNow
        };

        _db.Companies.Add(company);
        _db.Users.Add(user);

        await _db.SaveChangesAsync(cancellationToken);

        return new RegisterCompanyResponse(company.Id, user.Id);
    }
}
