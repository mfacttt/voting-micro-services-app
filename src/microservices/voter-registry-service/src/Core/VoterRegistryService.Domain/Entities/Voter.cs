using VoterRegistryService.Domain.Enums;

namespace VoterRegistryService.Domain.Entities;

public sealed class Voter
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required string NationalId { get; init; }

    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required DateOnly DateOfBirth { get; init; }

    public required string Country { get; init; }
    public string? Address { get; init; }

    public required bool IsResident { get; init; } = true;
    public bool IsSuspended { get; private set; }
    public VoterStatus Status { get; private set; } = VoterStatus.Active;

    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;

    public void Suspend()
    {
        Status = VoterStatus.Inactive;
        IsSuspended = true;
    }

    public void Activate()
    {
        Status = VoterStatus.Active;
        IsSuspended = false;
    }

    public void Delete()
    {
        Status = VoterStatus.Deleted;
        IsSuspended = true;
    }
}