using Infrastructure.Persistence;
using Infrastructure.Persistence.Seeders;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions;

public static class WebApplicationExtensions
{
    public static async Task InitializeDatabaseAsync(
        this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var db = scope.ServiceProvider
            .GetRequiredService<SupportFlowDbContext>();

        await db.Database.MigrateAsync();

        var seeder = scope.ServiceProvider
            .GetRequiredService<SuperAdminSeeder>();

        await seeder.SeedAsync();
    }
}