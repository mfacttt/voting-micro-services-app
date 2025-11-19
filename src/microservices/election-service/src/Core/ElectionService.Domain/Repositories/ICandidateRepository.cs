using ElectionService.Domain.Entities;

namespace ElectionService.Domain.Repositories;

public interface ICandidateRepository
{
    void Add(Candidate candidate, CancellationToken ct = default);
    void Remove(Candidate candidate, CancellationToken ct = default);

    Task<Candidate?> GetByIdAsync(Guid id, CancellationToken ct = default);

    Task<int> SaveChangesAsync(CancellationToken ct = default);
}