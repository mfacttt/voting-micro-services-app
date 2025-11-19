namespace ElectionService.Application.Contracts.Elections;

public record CreateElectionsRequest(
    string Name,
    string? Description,
    DateTimeOffset StartsAtUtc,
    DateTimeOffset EndsAtUtc);