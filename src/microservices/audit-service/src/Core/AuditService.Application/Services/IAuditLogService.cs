using AuditService.Application.Contracts.Requests;
using AuditService.Application.Contracts.Responses;

namespace AuditService.Application.Services;

public interface IAuditLogService
{
    Task CreateAuditLogAsync(AuditLogRequest request, CancellationToken ct = default);

    Task<IReadOnlyList<AuditLogResponse>> GetAuditLogsAsync(int pageNumber, int pageSize, CancellationToken ct = default);
}