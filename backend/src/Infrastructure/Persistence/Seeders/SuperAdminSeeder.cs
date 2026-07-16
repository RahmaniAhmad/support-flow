using Infrastructure.Authentication;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Shared.Authentication;
using Shared.Domain;
using Shared.Domain.Users;

namespace Infrastructure.Persistence.Seeders;

public sealed class SuperAdminSeeder
{
    private readonly SupportFlowDbContext _db;
    private readonly IPasswordHasher _passwordHasher;
    private readonly SuperAdminOptions _options;
    public SuperAdminSeeder(
        SupportFlowDbContext db,
        IPasswordHasher passwordHasher,
        IOptions<SuperAdminOptions> options)
    {
        _db = db;
        _passwordHasher = passwordHasher;
        _options = options.Value;
    }

    public async Task SeedAsync(
        CancellationToken cancellationToken = default)
    {
        var exists = await _db.Users
            .AnyAsync(
                x => x.Role == UserRole.SuperAdmin,
                cancellationToken);

        if (exists)
        {
            return;
        }

        var superAdmin = User.Create(
            companyId: null,
            email: _options.Email,
            passwordHash: _passwordHasher.Hash(_options.Password),
            role: UserRole.SuperAdmin);

        _db.Users.Add(superAdmin);

        await _db.SaveChangesAsync(cancellationToken);
    }
}