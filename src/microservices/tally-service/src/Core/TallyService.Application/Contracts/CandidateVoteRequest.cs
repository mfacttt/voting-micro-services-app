namespace TallyService.Application.Contracts;

public record CandidateVoteRequest(Guid ElectionId, Guid CandidateId);