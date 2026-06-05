using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Domain;

namespace Infrastructure.Persistence.Configurations
{
    public sealed class KnowledgeArticleConfiguration
        : IEntityTypeConfiguration<KnowledgeArticle>
    {
        public void Configure(
            EntityTypeBuilder<KnowledgeArticle> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title)
                .HasMaxLength(300);

            builder.Property(x => x.Content)
                .HasMaxLength(50000);

            builder.Property(x => x.CreatedAtUtc)
                .IsRequired();
        }
    }
}