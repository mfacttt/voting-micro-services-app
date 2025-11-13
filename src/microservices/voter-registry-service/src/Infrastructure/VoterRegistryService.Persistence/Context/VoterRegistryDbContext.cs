using Microsoft.EntityFrameworkCore;
using VoterRegistryService.Domain.Entities;

namespace VoterRegistryService.Persistence.Context;

public class VoterRegistryDbContext(DbContextOptions<VoterRegistryDbContext> options) : DbContext(options)
{
    public DbSet<Voter> Voters => Set<Voter>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(VoterRegistryDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}