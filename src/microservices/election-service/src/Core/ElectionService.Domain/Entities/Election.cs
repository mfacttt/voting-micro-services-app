using ElectionService.Domain.Enums;

namespace ElectionService.Domain.Entities;

public sealed class Election
{
    public Guid Id { get; init; } = Guid.CreateVersion7();
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedAt { get; private set; }

    public required string Name { get; set; }
    public string? Description { get; set; }

    public required DateTimeOffset StartsAtUtc { get; set; }
    public required DateTimeOffset EndsAtUtc { get; set; }

    public ElectionStatus Status { get; private set; } = ElectionStatus.Scheduled;

    public ICollection<Candidate> Candidates { get; init; } = new List<Candidate>();
    

    public void Activate(DateTimeOffset nowUtc)
    {
        Status = ElectionStatus.Active;
        UpdatedAt = nowUtc;
    }

    public void End(DateTimeOffset nowUtc)
    {
        Status = ElectionStatus.Ended;
        UpdatedAt = nowUtc;
    }

    public void Cancel(DateTimeOffset nowUtc)
    {
        Status = ElectionStatus.Cancelled;
        UpdatedAt = nowUtc;
    }


    public void AddCandidate(Candidate candidate)
    {
        if (Status != ElectionStatus.Scheduled)
        {
            throw new InvalidOperationException("Candidates can only be modified in Scheduled status.");
        }

        if (candidate.ElectionId != Id)
        {
            throw new InvalidOperationException("Candidate belongs to different election.");
        }

        Candidates.Add(candidate);
    }
}