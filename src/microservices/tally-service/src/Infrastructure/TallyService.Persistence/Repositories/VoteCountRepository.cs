using Microsoft.EntityFrameworkCore;
using TallyService.Domain.Entities;
using TallyService.Domain.Repositories;
using TallyService.Persistence.Context;

namespace TallyService.Persistence.Repositories;

public sealed class VoteCountRepository(TallyDbContext tallyDbContext) : IVoteCountRepository
{
    public async Task AddAsync(VoteCount voteCount, CancellationToken ct = default)
    {
        var existing = await tallyDbContext.VoteCounts
            .FirstOrDefaultAsync(x =>
                x.ElectionId == voteCount.ElectionId &&
                x.CandidateId == voteCount.CandidateId, ct);

        if (existing is null)
        {
            await tallyDbContext.VoteCounts.AddAsync(voteCount, ct);
            voteCount.Count++;
        }
        else
        {
            existing.Count++;
        }
    }


    public Task<VoteCount?> GetAsync(Guid electionId, Guid candidateId, CancellationToken ct = default)
    {
        return tallyDbContext.VoteCounts.FirstOrDefaultAsync(x=>
            x.ElectionId == electionId && x.CandidateId == candidateId, ct);
    }

    public async Task<IReadOnlyList<VoteCount>> GetByElectionAsync(Guid electionId, CancellationToken ct = default)
    {
        return await tallyDbContext.VoteCounts
            .Where(x => x.ElectionId == electionId)
            .ToListAsync(ct);
    }

    public Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return tallyDbContext.SaveChangesAsync(ct);
    }
}