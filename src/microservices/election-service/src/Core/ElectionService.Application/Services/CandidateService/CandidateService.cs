using ElectionService.Application.Contracts.Candidate;
using ElectionService.Domain.Entities;
using ElectionService.Domain.Repositories;

namespace ElectionService.Application.Services.CandidateService;

public sealed class CandidateService(IElectionRepository electionRepository, ICandidateRepository candidateRepository)
    : ICandidateService
{
    public async Task<CandidateResponse> CreateAsync(Guid electionId, CreateCandidateRequest request, CancellationToken ct = default)
    {
        var election = await electionRepository.GetByIdAsync(electionId, ct)
                       ?? throw new KeyNotFoundException("Election not found.");

        if (election.Status != Domain.Enums.ElectionStatus.Scheduled)
        {
            throw new InvalidOperationException("Candidates can only be added to Scheduled elections.");
        }

        var candidate = MapToCandidateEntity(electionId, request);

        candidateRepository.Add(candidate, ct);
        await candidateRepository.SaveChangesAsync(ct);

        var response = MapToCandidateResponse(candidate);

        return response;
    }

    public async Task<CandidateResponse?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var candidate = await candidateRepository.GetByIdAsync(id, ct);
        return candidate is null
            ? null
            : MapToCandidateResponse(candidate);
    }

    public async Task<IReadOnlyList<CandidateResponse>> GetByElectionIdAsync(Guid electionId, CancellationToken ct)
    {
        var election = await electionRepository.GetByIdAsync(electionId, ct);
        if (election is null || !election.Candidates.Any())
        {
            return [];
        }

        return election.Candidates.Select(MapToCandidateResponse).ToList();
    }

    public async Task DeleteAsync(Guid electionId, Guid id, CancellationToken ct)
    {
        var election = await electionRepository.GetByIdAsync(electionId, ct)
                       ?? throw new KeyNotFoundException("Election not found.");

        if (election.Status != Domain.Enums.ElectionStatus.Scheduled)
        {
            throw new InvalidOperationException("Candidates can only be deleted from Scheduled elections.");
        }

        var candidate = await candidateRepository.GetByIdAsync(id, ct);

        if (candidate is null)
        {
            return;
        }

        if (candidate.ElectionId != electionId)
        {
            throw new InvalidOperationException("Candidate does not belong to the specified election.");
        }

        candidateRepository.Remove(candidate, ct);
        await candidateRepository.SaveChangesAsync(ct);
    }


    private Candidate MapToCandidateEntity(Guid electionId, CreateCandidateRequest request)
    {
        return new Candidate
        {
            ElectionId = electionId,
            FullName = request.FullName,
            Party = request.Party,
            Description = request.Description
        };
    }

    private CandidateResponse MapToCandidateResponse(Candidate candidate)
    {
        return new CandidateResponse(
            candidate.Id,
            candidate.ElectionId,
            candidate.FullName,
            candidate.Party,
            candidate.Description,
            candidate.IsActive,
            candidate.CreatedAt);
    }
}