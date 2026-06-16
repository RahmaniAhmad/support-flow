using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Domain.Tickets;

namespace Infrastructure.Persistence.Configurations;

public sealed class TicketCommentConfiguration
    : IEntityTypeConfiguration<TicketComment>
{
    public void Configure(
        EntityTypeBuilder<TicketComment> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Content)
            .HasMaxLength(4000);

        builder.HasOne(x => x.Ticket)
            .WithMany()
            .HasForeignKey(x => x.TicketId);
    }
}
