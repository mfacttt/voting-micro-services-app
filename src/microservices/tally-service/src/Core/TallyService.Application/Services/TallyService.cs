using TallyService.Application.Contracts;
using TallyService.Domain.Entities;
using TallyService.Domain.Repositories;

namespace TallyService.Application.Services;

public class TallyService(IVoteCountRepository voteCountRepository) : ITallyService
{
    public async Task AddVoteAsync(CandidateVoteRequest request, CancellationToken ct = default)
    {
        await voteCountRepository.AddAsync(MapToVoteCountEntity(request), ct);
        await voteCountRepository.SaveChangesAsync(ct);
    }

    public async Task FinalizeElectionAsync(Guid electionId, CancellationToken ct = default)
    {
        var candidates = await voteCountRepository.GetByElectionAsync(electionId, ct);
        
        foreach (var candidate in candidates)
            candidate.FinalizeVoting();
        
        await voteCountRepository.SaveChangesAsync(ct);
    }

    public async Task<IReadOnlyList<CandidateCountResponse>> GetTallyAsync(Guid electionId, CancellationToken ct = default)
    {
        var candidates = await voteCountRepository.GetByElectionAsync(electionId, ct);
        return candidates.Select(MapToCandidateCountResponse).ToList();
    }
    
    private VoteCount MapToVoteCountEntity(CandidateVoteRequest request)
    {
        return new VoteCount
        {
            ElectionId = request.ElectionId,
            CandidateId = request.CandidateId,
        };
    }

    private CandidateCountResponse MapToCandidateCountResponse(VoteCount request)
    {
        return new CandidateCountResponse(
            request.ElectionId, 
            request.CandidateId, 
            request.Count);
    }
}