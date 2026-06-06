using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Shared.Domain;

namespace Infrastructure.Persistence.Configurations
{

    public sealed class TicketConfiguration
        : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(
            EntityTypeBuilder<Ticket> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Subject)
                .HasMaxLength(200);

            builder.Property(x => x.Description)
                .HasMaxLength(4000);

            builder.Property(x => x.Status)
                .HasConversion<string>();

            builder.Property(x => x.CreatedAtUtc)
                .IsRequired();

            builder.HasIndex(x => x.CompanyId);

            builder.HasIndex(x => x.AssignedToUserId);
        }
    }
}