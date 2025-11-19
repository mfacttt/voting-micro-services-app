using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TallyService.Domain.Entities;

namespace TallyService.Persistence.Configurations;

public sealed class VoteCountConfiguration : IEntityTypeConfiguration<VoteCount>
{
    public void Configure(EntityTypeBuilder<VoteCount> b)
    {
        b.HasKey(x => x.Id);
        
        b.Property(x => x.ElectionId)
            .IsRequired()
            .ValueGeneratedNever();
        
        b.Property(x => x.CandidateId)
            .IsRequired()
            .ValueGeneratedNever();
        
        b.HasIndex(x => new { x.ElectionId, x.CandidateId }).IsUnique();
    }
}