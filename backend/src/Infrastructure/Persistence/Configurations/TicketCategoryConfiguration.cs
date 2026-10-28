using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Domain.Tickets;

namespace Infrastructure.Persistence.Configurations;

public sealed class TicketCategoryConfiguration
    : IEntityTypeConfiguration<TicketCategory>
{
    public void Configure(
        EntityTypeBuilder<TicketCategory> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.CompanyId)
            .IsRequired();

        builder.Property(x => x.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property<string>("NormalizedName")
            .HasComputedColumnSql("lower(\"Name\")", stored: true)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(500);

        builder.Property(x => x.IsActive)
            .IsRequired();

        builder.Property(x => x.CreatedAtUtc)
            .IsRequired();

        builder.Property(x => x.UpdatedAtUtc);

        builder.HasAlternateKey(x => new
        {
            x.CompanyId,
            x.Id
        });

        builder.HasIndex("CompanyId", "NormalizedName")
            .IsUnique();

        builder.HasIndex(x => new
        {
            x.CompanyId,
            x.IsActive
        });
    }
}