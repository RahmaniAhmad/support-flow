using Infrastructure.Persistence;
using MediatR;
using Shared.Authentication;
using Shared.Domain;
using Shared.Domain.Users;

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
        var company = Company.Create(request.CompanyName);

        var user = User.Create
        (
             company.Id,
             request.Email,
             _passwordHasher.Hash(request.Password),
             UserRole.Admin
        );

        _db.Companies.Add(company);
        _db.Users.Add(user);

        await _db.SaveChangesAsync(cancellationToken);

        return new RegisterCompanyResponse(company.Id, user.Id);
    }
}
