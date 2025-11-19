using Microsoft.AspNetCore.Mvc;
using TallyService.Application.Contracts;
using TallyService.Application.Services;

namespace TallyService.Api.Controllers;

[ApiController]
[Route("api/tally")]
public sealed class TallyController(ITallyService tallyService) : ControllerBase
{
    [HttpPost("votes")]
    public async Task<IActionResult> Receive([FromBody] CandidateVoteRequest request, CancellationToken ct)
    {
        await tallyService.AddVoteAsync(request, ct);
        return Ok();
    }
    
    [HttpGet("{electionId:guid}")]
    public async Task<ActionResult> Get(Guid electionId, CancellationToken ct)
    {
        var response = await tallyService.GetTallyAsync(electionId, ct);
        return Ok(response);
    }
}