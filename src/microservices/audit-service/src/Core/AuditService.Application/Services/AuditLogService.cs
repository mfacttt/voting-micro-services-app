using AuditService.Application.Contracts.Requests;
using AuditService.Application.Contracts.Responses;
using AuditService.Domain.Abstractions;

namespace AuditService.Application.Services;

public class AuditLogService(IAuditLogRepository auditLogRepository) : IAuditLogService
{
    public Task CreateAuditLogAsync(AuditLogRequest request, CancellationToken ct = default)
    {
        return auditLogRepository.AddAsync(MapToEntity(request), ct);
    }

    public async Task<IReadOnlyList<AuditLogResponse>> GetAuditLogsAsync(int pageNumber, int pageSize, CancellationToken ct = default)
    {
        var logs = await auditLogRepository.GetPagedAsync((pageNumber - 1) * pageSize, pageSize, ct);
        return logs.Select(MapToResponse).ToList();
    }

    private AuditLogResponse MapToResponse(AuditLog log)
    {
        return new AuditLogResponse(
            log.Id,
            log.ServiceName,
            log.ActionName,
            log.Message,
            log.Severity,
            log.Timestamp);
    }

    private AuditLog MapToEntity(AuditLogRequest request)
    {
        return new AuditLog
        {
            ServiceName = request.ServiceName,
            ActionName = request.ActionName,
            Message = request.Message,
            Severity = request.Severity,
            Timestamp = DateTimeOffset.UtcNow
        };
    }
}