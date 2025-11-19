using ElectionService.Domain.Entities;

namespace ElectionService.Domain.Repositories;

public interface IElectionRepository
{
    void Add(Election election, CancellationToken ct = default);

    Task<Election?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<Election>> GetPagedAsync(int skip, int take, CancellationToken ct = default);

    Task<int> SaveChangesAsync(CancellationToken ct = default);
}