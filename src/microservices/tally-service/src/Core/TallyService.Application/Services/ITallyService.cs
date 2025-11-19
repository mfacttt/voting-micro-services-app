using TallyService.Application.Contracts;

namespace TallyService.Application.Services;

public interface ITallyService
{
    Task AddVoteAsync(CandidateVoteRequest request, CancellationToken ct = default);
    Task FinalizeElectionAsync(Guid electionId, CancellationToken ct = default);
    Task<IReadOnlyList<CandidateCountResponse>> GetTallyAsync(Guid electionId, CancellationToken ct = default);
}