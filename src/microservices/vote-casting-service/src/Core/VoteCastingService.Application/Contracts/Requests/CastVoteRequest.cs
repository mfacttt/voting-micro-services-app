namespace VoteCastingService.Application.Contracts.Requests;

public sealed record CastVoteRequest(
    Guid ElectionId,
    Guid VoterId,
    Guid CandidateId);