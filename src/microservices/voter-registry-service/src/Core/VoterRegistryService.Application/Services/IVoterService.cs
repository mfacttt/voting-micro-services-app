using VoterRegistryService.Application.Contracts.Requests;
using VoterRegistryService.Application.Contracts.Responses;
using VoterRegistryService.Domain.Entities;

namespace VoterRegistryService.Application.Services;

public interface IVoterService
{
    Task<VoterResponse> CreateAsync(CreateVoterRequest command, CancellationToken ct);
    Task<Voter?> UpdateStatusAsync(UpdateVoterStatusRequest command, CancellationToken ct);

    Task<VoterResponse?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<VoterResponse?> GetByNationalIdAsync(string nationalId, CancellationToken ct);
    Task<IReadOnlyList<VoterResponse>> GetPagedAsync(int page, int pageSize, CancellationToken ct);
}