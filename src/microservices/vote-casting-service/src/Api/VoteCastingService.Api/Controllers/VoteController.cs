using Microsoft.AspNetCore.Mvc;
using VoteCastingService.Application.Contracts.Requests;
using VoteCastingService.Application.Services;

namespace VoteCastingService.Api.Controllers;

[ApiController]
[Route("api/votes")]
public sealed class VoteController(IVoteCastingService service) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AddVote(CastVoteRequest request, CancellationToken ct)
    {
        try
        {
            await service.AddVoteAsync(request, ct);
            return StatusCode(StatusCodes.Status201Created, true);
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                success = false,
                message = ex.Message
            });
        }
    }

}