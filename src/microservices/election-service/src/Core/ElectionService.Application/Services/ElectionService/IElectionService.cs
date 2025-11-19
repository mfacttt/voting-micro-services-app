using ElectionService.Application.Contracts.Elections;
using ElectionService.Domain.Enums;

namespace ElectionService.Application.Services.ElectionService;

public interface IElectionService
{
    Task<ElectionsResponse> CreateAsync(CreateElectionsRequest request, CancellationToken ct = default);

    Task<ElectionsResponse?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<ElectionsResponse> ChangeStatusAsync(Guid electionId, ElectionStatus newStatus, CancellationToken ct = default);
}