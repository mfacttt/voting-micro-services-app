using VoteCastingService.Application.Contracts.Requests;
using VoteCastingService.Domain.Entities;
using VoteCastingService.Domain.Repositories;

namespace VoteCastingService.Application.Services;

public class VoteCastingService(IVoteRepository repository) : IVoteCastingService
{
    public async Task AddVoteAsync(CastVoteRequest request, CancellationToken ct = default)
    {
        var hasVoted = await repository.HasVotedAsync(request.ElectionId, request.VoterId, ct);
        if (hasVoted)
        {
            throw new InvalidOperationException("Voter has already cast a vote in this election.");
        }

        var vote = MapToVoteEntity(request);

        repository.AddAsync(vote, ct);
        await repository.SaveChangesAsync(ct);
    }

    private Vote MapToVoteEntity(CastVoteRequest request)
    {
        return new Vote
        {
            ElectionId = request.ElectionId,
            VoterId = request.VoterId,
            CandidateId = request.CandidateId,
            VoteAt = DateTimeOffset.UtcNow
        };
    }
}