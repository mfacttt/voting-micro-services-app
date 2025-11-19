using ElectionService.Domain.Entities;
using ElectionService.Domain.Repositories;
using ElectionService.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace ElectionService.Persistence.Repositories;

public sealed class CandidateRepository(ElectionDbContext context) : ICandidateRepository
{
    public Task<Candidate?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return context.Candidates.FirstOrDefaultAsync(c => c.Id == id, ct);
    }

    public void Add(Candidate candidate, CancellationToken ct)
    {
        context.Candidates.Add(candidate);
    }

    public void Remove(Candidate candidate, CancellationToken ct)
    {
        context.Candidates.Remove(candidate);
    }

    public Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return context.SaveChangesAsync(ct);
    }
}