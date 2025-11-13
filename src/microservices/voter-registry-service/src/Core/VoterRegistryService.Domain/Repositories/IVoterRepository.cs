using VoterRegistryService.Domain.Entities;

namespace VoterRegistryService.Domain.Repositories;

public interface IVoterRepository
{
    void CreateAsync(Voter voter, CancellationToken ct);
    void UpdateAsync(Voter voter, CancellationToken ct);

    Task<Voter?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<Voter?> GetByNationalIdAsync(string nationalId, CancellationToken ct);
    Task<IReadOnlyList<Voter>> GetPagedAsync(int skip, int take, CancellationToken ct);

    Task<bool> ExistsByIdAsync(Guid id, CancellationToken ct);
    Task<bool> ExistsByNationalIdAsync(string nationalId, CancellationToken ct);

    Task SaveChangesAsync(CancellationToken ct);
}