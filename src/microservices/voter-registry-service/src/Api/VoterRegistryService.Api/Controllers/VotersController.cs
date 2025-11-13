using Microsoft.AspNetCore.Mvc;
using VoterRegistryService.Application.Contracts.Requests;
using VoterRegistryService.Application.Services;

namespace VoterRegistryService.Api.Controllers;

[ApiController]
[Route("api/v1/voters")]
public class VotersController(IVoterService service) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateVoterRequest request, CancellationToken ct)
    {
        var voter = await service.CreateAsync(request, ct);

        return StatusCode(StatusCodes.Status201Created, voter);
    }

    [HttpPatch("{id:guid}/status")]
    public async Task<IActionResult> UpdateStatusAsync(Guid id, UpdateVoterStatusRequest request, CancellationToken ct)
    {
        var updated = await service.UpdateStatusAsync(request, ct);
        return updated is null
            ? NotFound()
            : Ok(updated);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var voter = await service.GetByIdAsync(id, ct);
        return voter is null
            ? NotFound()
            : Ok(voter);
    }

    [HttpGet("national/{nationalId}")]
    public async Task<IActionResult> GetByNationalIdAsync(string nationalId, CancellationToken ct)
    {
        var voter = await service.GetByNationalIdAsync(nationalId, ct);
        return voter is null
            ? NotFound()
            : Ok(voter);
    }

    [HttpGet]
    public async Task<IActionResult> GetPagedAsync([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
    {
        if (page <= 0) page = 1;
        if (pageSize <= 0 || pageSize > 100) pageSize = 20;

        var voters = await service.GetPagedAsync(page, pageSize, ct);
        return Ok(voters);
    }
}