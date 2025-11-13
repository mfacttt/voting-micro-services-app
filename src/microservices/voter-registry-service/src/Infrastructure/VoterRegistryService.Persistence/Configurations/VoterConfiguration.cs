using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VoterRegistryService.Domain.Entities;

namespace VoterRegistryService.Persistence.Configurations;

public class VoterConfiguration : IEntityTypeConfiguration<Voter>
{
    public void Configure(EntityTypeBuilder<Voter> builder)
    {
        builder.HasKey(v => v.Id);

        builder.Property(v => v.NationalId)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(v => v.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(v => v.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(v => v.Country)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(v => v.Address)
            .HasMaxLength(256);

        builder.HasIndex(v => v.NationalId)
            .IsUnique();
    }
}