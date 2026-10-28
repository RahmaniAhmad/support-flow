using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Shared.Domain.Base;
using Shared.Domain.Tickets;

namespace UnitTests.Tickets;

public sealed class TicketCategoryModelTests
{
    [Fact]
    public void Category_name_is_unique_per_company_case_insensitively()
    {
        using var context = CreateContext();

        var categoryType = context.Model.FindEntityType(
            typeof(TicketCategory))!;
        var uniqueIndex = Assert.Single(
            categoryType.GetIndexes(),
            index => index.IsUnique);
        var indexProperties = uniqueIndex.Properties
            .Select(property => property.Name)
            .ToArray();

        Assert.Contains("CompanyId", indexProperties);
        Assert.Contains("NormalizedName", indexProperties);
        Assert.NotEqual("Name", indexProperties[^1]);
        Assert.True(
            categoryType.FindProperty("NormalizedName")!
                .GetComputedColumnSql()!
                .Contains("lower", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void Ticket_category_foreign_key_is_optional_and_restricts_deletion()
    {
        using var context = CreateContext();

        var ticketType = context.Model.FindEntityType(typeof(Ticket))!;
        var categoryForeignKey = Assert.Single(
            ticketType.GetForeignKeys(),
            foreignKey =>
                foreignKey.PrincipalEntityType.ClrType
                == typeof(TicketCategory));

        Assert.False(categoryForeignKey.IsRequired);
        Assert.Equal(DeleteBehavior.Restrict, categoryForeignKey.DeleteBehavior);
        Assert.Contains(
            categoryForeignKey.Properties,
            property => property.Name == nameof(Ticket.CategoryId));
        Assert.Contains(
            categoryForeignKey.Properties,
            property => property.Name == nameof(Ticket.CompanyId));
    }

    private static SupportFlowDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<SupportFlowDbContext>()
            .UseNpgsql(
                "Host=localhost;Database=model-tests;Username=test;Password=test",
                npgsqlOptions => npgsqlOptions.UseVector())
            .Options;

        return new SupportFlowDbContext(
            options,
            new NoOpDomainEventDispatcher());
    }

    private sealed class NoOpDomainEventDispatcher : IDomainEventDispatcher
    {
        public Task DispatchAsync(
            IEnumerable<IDomainEvent> domainEvents,
            CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
}
