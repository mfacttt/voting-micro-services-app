using Microsoft.EntityFrameworkCore;
using VoteCastingService.Domain.Entities;

namespace VoteCastingService.Persistence.Context;

public sealed class VoteDbContext(DbContextOptions<VoteDbContext> options) : DbContext(options)
{
    public DbSet<Vote> Votes => Set<Vote>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(VoteDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}