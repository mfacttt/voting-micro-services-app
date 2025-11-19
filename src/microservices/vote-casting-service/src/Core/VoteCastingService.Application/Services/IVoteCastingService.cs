using VoteCastingService.Application.Contracts.Requests;

namespace VoteCastingService.Application.Services;

public interface IVoteCastingService
{
    Task AddVoteAsync(CastVoteRequest request, CancellationToken ct = default);
}