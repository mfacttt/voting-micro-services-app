using ElectionService.Application.Contracts.Elections;
using ElectionService.Application.Services.ElectionService;
using ElectionService.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace ElectionService.Api.Controllers;

[ApiController]
[Route("api/elections")]
public sealed class ElectionsController(IElectionService electionService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateElectionsRequest request, CancellationToken ct)
    {
        var result = await electionService.CreateAsync(request, ct);

        return StatusCode(StatusCodes.Status201Created, result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await electionService.GetByIdAsync(id, ct);
        return result == null
            ? StatusCode(StatusCodes.Status404NotFound)
            : StatusCode(StatusCodes.Status200OK, result);
    }

    [HttpPost("{id:guid}/activate")]
    public async Task<ActionResult> Activate(Guid id, CancellationToken ct)
    {
        var result = await electionService.ChangeStatusAsync(id, ElectionStatus.Active, ct);
        return StatusCode(StatusCodes.Status200OK, result);
    }

    [HttpPost("{id:guid}/end")]
    public async Task<ActionResult> End(Guid id, CancellationToken ct)
    {
        var result = await electionService.ChangeStatusAsync(id, ElectionStatus.Ended, ct);
        return StatusCode(StatusCodes.Status200OK, result);
    }

    [HttpPost("{id:guid}/cancel")]
    public async Task<ActionResult> Cancel(Guid id, CancellationToken ct)
    {
        var result = await electionService.ChangeStatusAsync(id, ElectionStatus.Cancelled, ct);
        return StatusCode(StatusCodes.Status200OK, result);
    }
}