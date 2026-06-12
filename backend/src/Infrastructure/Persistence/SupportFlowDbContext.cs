using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Domain;
using Shared.Domain.Base;

namespace Infrastructure.Persistence;

public sealed class SupportFlowDbContext : DbContext
{
    private readonly IMediator _mediator;
    public SupportFlowDbContext(
        DbContextOptions<SupportFlowDbContext> options, IMediator mediator)
        : base(options) => _mediator = mediator;

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

        var entitiesWithEvents = ChangeTracker.Entries<Entity>()
            .Select(e => e.Entity)
            .Where(e => e.DomainEvents.Any())
            .ToList();

        foreach (var entity in entitiesWithEvents)
        {
            while (entity.DomainEvents.TryDequeue(out var domainEvent))
            {
                await _mediator.Publish(domainEvent, cancellationToken);
            }
        }

        return result;
    }
}