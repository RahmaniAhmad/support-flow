using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Domain;
using Shared.Domain.Companies;

namespace Infrastructure.Persistence.Configurations;

public sealed class CompanyTicketCounterConfiguration
    : IEntityTypeConfiguration<CompanyTicketCounter>
{
    public void Configure(
        EntityTypeBuilder<CompanyTicketCounter> builder)
    {
        builder.HasKey(x => x.Id);


        builder.Property(x => x.CompanyId)
            .IsRequired();


        builder.Property(x => x.LastTicketNumber)
            .IsRequired();


        builder.HasIndex(x => x.CompanyId)
            .IsUnique();
    }
}