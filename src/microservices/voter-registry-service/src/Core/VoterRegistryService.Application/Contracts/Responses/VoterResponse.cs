using VoterRegistryService.Domain.Enums;

namespace VoterRegistryService.Application.Contracts.Responses;

public record VoterResponse(
    Guid Id,
    string NationalId,
    string FirstName,
    string LastName,
    DateOnly DateOfBirth,
    string Country,
    string? Address,
    bool IsResident,
    VoterStatus Status,
    DateTimeOffset CreatedAt);