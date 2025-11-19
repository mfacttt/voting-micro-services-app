using Microsoft.Extensions.DependencyInjection;
using VoteCastingService.Domain.Repositories;
using VoteCastingService.Infrastructure.Clients;
using VoteCastingService.Persistence.Repositories;

namespace VoteCastingService.Infrastructure.Extensions;

public static class AddInfrastructure
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<IVoteRepository, VoteRepository>();

        services.AddScoped<IEligibilityClient, EligibilityClient>();
        services.AddScoped<IElectionStatusClient, ElectionStatusClient>();
        return services;
    }
}