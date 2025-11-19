using Microsoft.EntityFrameworkCore;
using VoteCastingService.Domain.Entities;
using VoteCastingService.Domain.Repositories;
using VoteCastingService.Persistence.Context;

namespace VoteCastingService.Persistence.Repositories;

public sealed class VoteRepository(VoteDbContext context) : IVoteRepository
{
    public void AddAsync(Vote vote, CancellationToken ct = default)
    {
        context.Votes.Add(vote);
    }

    public async Task<bool> HasVotedAsync(Guid electionId, Guid voterId, CancellationToken ct = default)
    {
        return await context.Votes.AnyAsync(x => x.ElectionId == electionId && x.VoterId == voterId, ct);
    }

    public Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return context.SaveChangesAsync(ct);
    }
}