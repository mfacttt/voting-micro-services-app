namespace AuditService.Domain.Abstractions;

public interface IAuditLogRepository
{
    Task AddAsync(AuditLog log, CancellationToken ct = default);

    Task<IReadOnlyList<AuditLog>> GetPagedAsync(int skip, int take, CancellationToken ct = default);
}