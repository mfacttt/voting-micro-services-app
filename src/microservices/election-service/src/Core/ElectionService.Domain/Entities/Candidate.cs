namespace ElectionService.Domain.Entities;

public sealed class Candidate
{
    public Guid Id { get; init; } = Guid.CreateVersion7();

    public required Guid ElectionId { get; init; }

    public required string FullName { get; init; }
    public string? Party { get; init; }
    public string? Description { get; init; }
    public bool IsActive { get; private set; } = true;

    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;

    public void Deactivate()
    {
        IsActive = false;
    }
}