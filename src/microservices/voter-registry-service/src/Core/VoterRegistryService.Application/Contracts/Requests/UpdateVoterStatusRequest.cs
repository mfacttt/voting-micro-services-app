using VoterRegistryService.Domain.Enums;

namespace VoterRegistryService.Application.Contracts.Requests;

public sealed record UpdateVoterStatusRequest(
    Guid VoterId,
    VoterStatus Status);