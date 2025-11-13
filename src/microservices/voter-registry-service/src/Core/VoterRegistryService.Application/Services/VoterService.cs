using VoterRegistryService.Application.Contracts.Requests;
using VoterRegistryService.Application.Contracts.Responses;
using VoterRegistryService.Domain.Entities;
using VoterRegistryService.Domain.Enums;
using VoterRegistryService.Domain.Repositories;

namespace VoterRegistryService.Application.Services;

public class VoterService(IVoterRepository voterRepository) : IVoterService
{
    public async Task<VoterResponse> CreateAsync(CreateVoterRequest command, CancellationToken ct)
    {
        if (await voterRepository.ExistsByNationalIdAsync(command.NationalId, ct))
        {
            throw new InvalidOperationException($"Voter with National ID {command.NationalId} already exists.");
        }

        var voter = MapToVoterEntity(command);
        voter.Activate();

        voterRepository.CreateAsync(voter, ct);
        await voterRepository.SaveChangesAsync(ct);

        return MapToVoterResponse(voter);
    }

    public async Task<Voter?> UpdateStatusAsync(UpdateVoterStatusRequest command, CancellationToken ct)
    {
        var voter = await voterRepository.GetByIdAsync(command.VoterId, ct);

        if (voter is null)
        {
            throw new InvalidOperationException($"Voter with ID {command.VoterId} does not exist.");
        }

        switch (command.Status)
        {
            case VoterStatus.Active:
                voter.Activate();
                break;
            case VoterStatus.Inactive:
                voter.Suspend();
                break;
            case VoterStatus.Deleted:
                voter.Delete();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(command.Status), $"Unsupported status: {command.Status}");
        }

        voterRepository.UpdateAsync(voter, ct);
        await voterRepository.SaveChangesAsync(ct);

        return voter;
    }

    public async Task<VoterResponse?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var voter = await voterRepository.GetByIdAsync(id, ct);
        return voter is null
            ? null
            : MapToVoterResponse(voter);
    }

    public async Task<VoterResponse?> GetByNationalIdAsync(string nationalId, CancellationToken ct)
    {
        var voter = await voterRepository.GetByNationalIdAsync(nationalId, ct);
        return voter is null
            ? null
            : MapToVoterResponse(voter);
    }

    public async Task<IReadOnlyList<VoterResponse>> GetPagedAsync(int page, int pageSize, CancellationToken ct)
    {
        var skip = (page - 1) * pageSize;

        var voters = await voterRepository.GetPagedAsync(skip, pageSize, ct);

        return voters
            .Select(MapToVoterResponse)
            .ToList();
    }

    private Voter MapToVoterEntity(CreateVoterRequest command)
    {
        return new Voter
        {
            Id = Guid.CreateVersion7(),
            NationalId = command.NationalId,
            FirstName = command.FirstName,
            LastName = command.LastName,
            DateOfBirth = command.DateOfBirth,
            Country = command.Country,
            Address = command.Address,
            IsResident = command.IsResident,
            CreatedAt = DateTime.UtcNow
        };
    }

    private VoterResponse MapToVoterResponse(Voter voter)
    {
        return new VoterResponse(
            voter.Id,
            voter.NationalId,
            voter.FirstName,
            voter.LastName,
            voter.DateOfBirth,
            voter.Country,
            voter.Address,
            voter.IsResident,
            voter.Status,
            voter.CreatedAt);
    }
}