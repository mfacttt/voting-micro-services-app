using ElectionService.Application.Contracts.Candidate;
using ElectionService.Application.Services.CandidateService;
using Microsoft.AspNetCore.Mvc;

namespace ElectionService.Api.Controllers;

[ApiController]
[Route("api/elections/{electionId:guid}/candidates")]
public sealed class CandidatesController(ICandidateService candidateService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> Create(Guid electionId, [FromBody] CreateCandidateRequest request, CancellationToken ct)
    {
        var result = await candidateService.CreateAsync(electionId, request, ct);

        return StatusCode(StatusCodes.Status201Created, result);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CandidateResponse>>> GetAll(Guid electionId, CancellationToken ct)
    {
        var candidates = await candidateService.GetByElectionIdAsync(electionId, ct);
        return Ok(candidates);
    }

    [HttpDelete("{candidateId:guid}")]
    public async Task<ActionResult> Delete(Guid electionId, Guid candidateId, CancellationToken ct)
    {
        await candidateService.DeleteAsync(electionId, candidateId, ct);
        return NoContent();
    }
}