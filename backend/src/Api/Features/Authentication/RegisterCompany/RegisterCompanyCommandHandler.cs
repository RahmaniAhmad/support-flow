using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Authentication;
using Shared.Domain;
using Shared.Domain.Companies;
using Shared.Domain.Users;

namespace Api.Features.Authentication.RegisterCompany;

public class RegisterCompanyCommandHandler
    : IRequestHandler<RegisterCompanyCommand>
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

    public async Task Handle(
        RegisterCompanyCommand request,
        CancellationToken cancellationToken)
    {
        var emailExists = await _db.Users
        .AnyAsync(u => u.Email == request.Email, cancellationToken);

        if (emailExists)
        {
            throw new InvalidOperationException("Email is already registered.");
        }

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
    }
}
