using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VoteCastingService.Domain.Entities;

namespace VoteCastingService.Persistence.Configurations;

public class VoteConfiguration : IEntityTypeConfiguration<Vote>
{
    public void Configure(EntityTypeBuilder<Vote> builder)
    {
        builder.HasKey(v => v.Id);

        builder.Property(v => v.VoterId)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(v => v.ElectionId)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(v => v.CandidateId)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(v => v.VoteAt)
            .IsRequired();
    }
}