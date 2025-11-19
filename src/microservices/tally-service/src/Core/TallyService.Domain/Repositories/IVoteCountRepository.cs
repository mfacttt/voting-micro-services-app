using TallyService.Domain.Entities;

namespace TallyService.Domain.Repositories;

public interface IVoteCountRepository
{
    Task AddAsync(VoteCount voteCount, CancellationToken ct = default);

    Task<VoteCount?> GetAsync(Guid electionId, Guid candidateId, CancellationToken ct = default);
    Task<IReadOnlyList<VoteCount>> GetByElectionAsync(Guid electionId, CancellationToken ct = default);

    Task<int> SaveChangesAsync(CancellationToken ct = default);
}