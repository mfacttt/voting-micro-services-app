namespace VoteCastingService.Infrastructure.Clients;

public interface IElectionStatusClient
{
    Task<bool> IsElectionActiveAsync(Guid electionId, CancellationToken ct = default);
}

public sealed class ElectionStatusClient : IElectionStatusClient
{
    public Task<bool> IsElectionActiveAsync(Guid id, CancellationToken ct = default)
    {
        return Task.FromResult(true);
    }
}