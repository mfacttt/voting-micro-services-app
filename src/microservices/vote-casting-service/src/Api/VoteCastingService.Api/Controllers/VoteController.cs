using Microsoft.AspNetCore.Mvc;
using VoteCastingService.Application.Contracts.Requests;

namespace VoteCastingService.Api.Controllers;

[ApiController]
[Route("api/votes")]
public sealed class VoteController(Application.Services.VoteCastingService service) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AddVote(CastVoteRequest request, CancellationToken ct)
    {
        await service.AddVoteAsync(request, ct);
        return StatusCode(StatusCodes.Status201Created, true);
    }
}