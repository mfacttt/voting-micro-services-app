using ElectionService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ElectionService.Persistence.Context;

public sealed class ElectionDbContext(DbContextOptions<ElectionDbContext> options)
    : DbContext(options)
{
    public DbSet<Election> Elections => Set<Election>();
    public DbSet<Candidate> Candidates => Set<Candidate>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ElectionDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}