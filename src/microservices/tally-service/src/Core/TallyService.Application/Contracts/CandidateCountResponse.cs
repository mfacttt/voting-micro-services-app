namespace TallyService.Application.Contracts;

public record CandidateCountResponse(Guid ElectionId, Guid CandidateId, long Count);