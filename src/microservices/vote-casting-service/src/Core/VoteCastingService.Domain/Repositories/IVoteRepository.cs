using VoteCastingService.Domain.Entities;

namespace VoteCastingService.Domain.Repositories;

public interface IVoteRepository
{
    void AddAsync(Vote vote, CancellationToken ct = default);
    Task<bool> HasVotedAsync(Guid electionId, Guid voterId, CancellationToken ct = default);

    Task<int> SaveChangesAsync(CancellationToken ct = default);
}