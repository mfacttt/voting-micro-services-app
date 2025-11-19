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


    public bool IsActive(DateTimeOffset nowUtc)
    {
        return Status == ElectionStatus.Active &&
               nowUtc >= StartsAtUtc &&
               nowUtc <= EndsAtUtc;
    }

    public bool CanBeActivated(DateTimeOffset nowUtc)
    {
        return Status is ElectionStatus.Scheduled &&
               nowUtc >= StartsAtUtc &&
               nowUtc < EndsAtUtc;
    }


    public void Activate(DateTimeOffset nowUtc)
    {
        if (!CanBeActivated(nowUtc))
        {
            throw new InvalidOperationException("Election cannot be activated in current state.");
        }

        Status = ElectionStatus.Active;
        UpdatedAt = nowUtc;
    }

    public void End(DateTimeOffset nowUtc)
    {
        if (Status is not ElectionStatus.Active)
        {
            throw new InvalidOperationException("Only active elections can be ended.");
        }

        Status = ElectionStatus.Ended;
        UpdatedAt = nowUtc;
    }

    public void Cancel(DateTimeOffset nowUtc)
    {
        if (Status == ElectionStatus.Ended)
        {
            throw new InvalidOperationException("Cannot cancel ended election.");
        }

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