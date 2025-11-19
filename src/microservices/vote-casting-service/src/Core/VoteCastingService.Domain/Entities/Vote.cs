namespace VoteCastingService.Domain.Entities;

public sealed class Vote
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required Guid VoterId { get; init; }
    public required Guid ElectionId { get; init; }

    public DateTimeOffset VoteAt { get; init; } = DateTimeOffset.UtcNow;
}