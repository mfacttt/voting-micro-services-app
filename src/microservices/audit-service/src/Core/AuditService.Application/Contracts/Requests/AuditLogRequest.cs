using AuditService.Domain.Enums;

namespace AuditService.Application.Contracts.Requests;

public record AuditLogRequest(
    string ServiceName,
    string ActionName,
    string Message,
    AuditSeverity Severity);