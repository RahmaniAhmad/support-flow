using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Domain.AI;

namespace Infrastructure.Persistence.Configurations;

public sealed class EmbeddingDocumentConfiguration
    : IEntityTypeConfiguration<EmbeddingDocument>
{
    public void Configure(
        EntityTypeBuilder<EmbeddingDocument> builder)
    {
        builder.ToTable("EmbeddingDocuments");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.SourceType)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Content)
            .HasMaxLength(10000)
            .IsRequired();

        builder.Property(x => x.Vector)
            .HasColumnType("vector(768)")
            .IsRequired();

        builder.HasIndex(x => new
        {
            x.SourceId,
            x.SourceType
        });

        builder.HasIndex(x => x.CompanyId);
    }
}