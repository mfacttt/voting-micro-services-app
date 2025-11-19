using ElectionService.Domain.Entities;
using ElectionService.Domain.Repositories;
using ElectionService.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace ElectionService.Persistence.Repositories;

public sealed class ElectionRepository(ElectionDbContext context) : IElectionRepository
{
    public async Task<Election?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await context.Elections
            .Include(e => e.Candidates)
            .FirstOrDefaultAsync(e => e.Id == id, ct);
    }

    public async Task<IReadOnlyList<Election>> GetPagedAsync(int skip, int take, CancellationToken ct = default)
    {
        return await context.Elections
            .OrderByDescending(e => e.CreatedAt)
            .Skip(skip)
            .Take(take)
            .ToListAsync(ct);
    }

    public Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return context.SaveChangesAsync(ct);
    }

    public void Add(Election election, CancellationToken ct)
    {
        context.Elections.Add(election);
    }
}