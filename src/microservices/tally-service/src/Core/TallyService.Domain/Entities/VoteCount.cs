namespace TallyService.Domain.Entities;

public sealed class VoteCount
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required Guid ElectionId { get; init; }
    public required Guid CandidateId { get; init; }

    public long Count { get; set; }
    public bool IsFinal { get; private set; }
    
    public void Increment()
    {
        if (IsFinal) throw new InvalidOperationException("final");
        Count++;
    }

    public void FinalizeVoting()
    {
        IsFinal = true;
    }
}