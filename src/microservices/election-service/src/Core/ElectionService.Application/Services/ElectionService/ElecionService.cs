using ElectionService.Application.Contracts.Elections;
using ElectionService.Domain.Entities;
using ElectionService.Domain.Enums;
using ElectionService.Domain.Repositories;

namespace ElectionService.Application.Services.ElectionService;

public sealed class ElectionService(IElectionRepository electionRepository) : IElectionService
{
    public async Task<ElectionsResponse> CreateAsync(CreateElectionsRequest request, CancellationToken ct = default)
    {
        if (request.EndsAtUtc <= request.StartsAtUtc)
        {
            throw new ArgumentException("EndsAtUtc must be > StartsAtUtc");
        }

        var election = MapToElectionEntity(request);

        electionRepository.Add(election, ct);
        await electionRepository.SaveChangesAsync(ct);

        return MapToResponse(election);
    }

    public async Task<ElectionsResponse?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var election = await electionRepository.GetByIdAsync(id, ct);
        return election is null
            ? null
            : MapToResponse(election);
    }

    public async Task<ElectionsResponse> ChangeStatusAsync(Guid electionId, ElectionStatus newStatus, CancellationToken ct)
    {
        var election = await electionRepository.GetByIdAsync(electionId, ct)
                       ?? throw new KeyNotFoundException("Election not found.");

        var now = DateTimeOffset.UtcNow;

        switch (newStatus)
        {
            case ElectionStatus.Active:
                election.Activate(now);
                break;

            case ElectionStatus.Ended:
                election.End(now);
                break;

            case ElectionStatus.Cancelled:
                election.Cancel(now);
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(newStatus));
        }

        await electionRepository.SaveChangesAsync(ct);

        return MapToResponse(election);
    }


    private ElectionsResponse MapToResponse(Election election)
    {
        return new ElectionsResponse(
            election.Id,
            election.Name,
            election.Description,
            election.StartsAtUtc,
            election.EndsAtUtc,
            election.Status,
            election.Candidates.Count
        );
    }

    private Election MapToElectionEntity(CreateElectionsRequest request)
    {
        return new Election
        {
            Name = request.Name,
            Description = request.Description,
            StartsAtUtc = request.StartsAtUtc,
            EndsAtUtc = request.EndsAtUtc
        };
    }
}