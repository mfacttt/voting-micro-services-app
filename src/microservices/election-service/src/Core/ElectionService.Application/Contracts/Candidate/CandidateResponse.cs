namespace ElectionService.Application.Contracts.Candidate;

public record CandidateResponse(
    Guid Id,
    Guid ElectionId,
    string FullName,
    string? Party,
    string? Description,
    bool IsActive,
    DateTimeOffset CreatedAt);