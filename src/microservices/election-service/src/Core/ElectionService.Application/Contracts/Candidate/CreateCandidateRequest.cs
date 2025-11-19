namespace ElectionService.Application.Contracts.Candidate;

public record CreateCandidateRequest(
    string FullName,
    string? Party,
    string? Description);