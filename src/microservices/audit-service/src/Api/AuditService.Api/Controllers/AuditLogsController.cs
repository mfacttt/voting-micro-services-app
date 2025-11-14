using AuditService.Application.Contracts.Requests;
using AuditService.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace AuditService.Api.Controllers;

[ApiController]
[Route("api/audit-logs")]
public sealed class AuditLogsController(IAuditLogService logService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateAsync(AuditLogRequest request, CancellationToken ct)
    {
        await logService.CreateAuditLogAsync(request, ct);
        return StatusCode(StatusCodes.Status201Created);
    }

    [HttpGet]
    public async Task<IActionResult> GetPagedAsync([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
    {
        if (page <= 0) page = 1;
        if (pageSize <= 0 || pageSize > 100) pageSize = 20;
        var logs = await logService.GetAuditLogsAsync(page, pageSize, ct);
        return Ok(logs);
    }
}