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

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.Content)
            .HasMaxLength(4000)
            .IsRequired();

        builder.Property(x => x.CreatedAtUtc)
            .IsRequired();

        builder.HasIndex(x => x.TicketId);

        builder.HasIndex(x => x.AuthorUserId);

        builder.HasOne<Ticket>()
           .WithMany(x => x.Comments)
           .HasForeignKey(x => x.TicketId)
           .OnDelete(DeleteBehavior.Cascade);
    }
}
