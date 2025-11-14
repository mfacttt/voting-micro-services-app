using AuditService.Domain.Enums;

public sealed class AuditLog
{
    public Guid Id { get; init; } = Guid.CreateVersion7();

    public required string ServiceName { get; init; }
    public required string ActionName { get; init; }
    public required string Message { get; init; }

    public AuditSeverity Severity { get; init; }

    public required DateTimeOffset Timestamp { get; init; } = DateTimeOffset.UtcNow;
}