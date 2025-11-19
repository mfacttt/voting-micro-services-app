using ElectionService.Domain.Enums;

namespace ElectionService.Application.Contracts.Elections;

public record ElectionsResponse(
    Guid Id,
    string Name,
    string? Description,
    DateTimeOffset StartsAtUtc,
    DateTimeOffset EndsAtUtc,
    ElectionStatus Status,
    int CandidatesCount);