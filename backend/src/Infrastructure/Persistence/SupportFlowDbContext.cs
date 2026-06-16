using Microsoft.EntityFrameworkCore;
using Shared.Domain;
using Shared.Domain.Base;
using Shared.Domain.Tickets;

namespace Infrastructure.Persistence;

public sealed class SupportFlowDbContext : DbContext
{
    private readonly IDomainEventDispatcher _dispatcher;
    public SupportFlowDbContext(
        DbContextOptions<SupportFlowDbContext> options, IDomainEventDispatcher dispatcher)
        : base(options) => _dispatcher = dispatcher;

    public DbSet<Company> Companies => Set<Company>();

    public DbSet<User> Users => Set<User>();

    public DbSet<Ticket> Tickets => Set<Ticket>();

    public DbSet<TicketComment> TicketComments => Set<TicketComment>();

    public DbSet<KnowledgeArticle> KnowledgeArticles => Set<KnowledgeArticle>();

    protected override void OnModelCreating(
    ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(SupportFlowDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var result = await base.SaveChangesAsync(cancellationToken);

        var domainEvents = GetDomainEvents();

        await _dispatcher.DispatchAsync(
            domainEvents,
            cancellationToken);

        return result;
    }
    private List<IDomainEvent> GetDomainEvents()
    {
        return ChangeTracker
            .Entries<AggregateRoot>()
            .Select(x => x.Entity)
            .SelectMany(x =>
            {
                var events = x.DomainEvents.ToList();

                x.ClearDomainEvents();

                return events;
            })
            .ToList();
    }

}