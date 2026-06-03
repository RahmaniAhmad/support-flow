using Microsoft.EntityFrameworkCore;
using Shared.Domain;

namespace Infrastructure.Persistence;

public sealed class SupportFlowDbContext : DbContext
{
    public SupportFlowDbContext(
        DbContextOptions<SupportFlowDbContext> options)
        : base(options)
    {
    }

    public DbSet<Company> Companies => Set<Company>();

    public DbSet<User> Users => Set<User>();

    public DbSet<Ticket> Tickets => Set<Ticket>();

    public DbSet<TicketComment> TicketComments => Set<TicketComment>();

    public DbSet<KnowledgeArticle> KnowledgeArticles => Set<KnowledgeArticle>();
}