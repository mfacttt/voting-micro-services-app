using AuditService.Domain.Enums;

namespace AuditService.Application.Contracts.Responses;

public record AuditLogResponse(
    Guid Id,
    string ServiceName,
    string ActionName,
    string Message,
    AuditSeverity Severity,
    DateTimeOffset Timestamp);