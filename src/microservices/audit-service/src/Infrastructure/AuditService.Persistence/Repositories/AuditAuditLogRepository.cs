using AuditService.Domain.Abstractions;
using AuditService.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace AuditService.Persistence.Repositories;

public class AuditAuditLogRepository(AuditDbContext context) : IAuditLogRepository
{
    public Task AddAsync(AuditLog log, CancellationToken ct = default)
    {
        context.Logs.Add(log);
        return context.SaveChangesAsync(ct);
    }

    public async Task<IReadOnlyList<AuditLog>> GetPagedAsync(int skip, int take, CancellationToken ct = default)
    {
        return await context.Logs.AsNoTracking().Skip(skip).Take(take).ToListAsync(ct);
    }
}