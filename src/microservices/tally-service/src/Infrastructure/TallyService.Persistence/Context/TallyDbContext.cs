using Microsoft.EntityFrameworkCore;
using TallyService.Domain.Entities;

namespace TallyService.Persistence.Context;

public sealed class TallyDbContext(DbContextOptions<TallyDbContext> options) : DbContext(options)
{
    public DbSet<VoteCount> VoteCounts => Set<VoteCount>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TallyDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}