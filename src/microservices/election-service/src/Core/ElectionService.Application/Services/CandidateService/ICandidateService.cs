using ElectionService.Application.Contracts.Candidate;

namespace ElectionService.Application.Services.CandidateService;

public interface ICandidateService
{
    Task<CandidateResponse> CreateAsync(Guid electionId, CreateCandidateRequest request, CancellationToken ct = default);

    Task<CandidateResponse?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<CandidateResponse>> GetByElectionIdAsync(Guid electionId, CancellationToken ct = default);

    Task DeleteAsync(Guid electionId, Guid id, CancellationToken ct = default);
}