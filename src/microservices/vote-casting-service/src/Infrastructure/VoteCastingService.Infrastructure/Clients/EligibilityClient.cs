namespace VoteCastingService.Infrastructure.Clients;

public interface IEligibilityClient
{
    Task<bool> IsEligibleAsync(Guid voterId, CancellationToken ct = default);
}

public sealed class EligibilityClient : IEligibilityClient
{
    public Task<bool> IsEligibleAsync(Guid id, CancellationToken ct = default)
    {
        return Task.FromResult(true);
    }
}