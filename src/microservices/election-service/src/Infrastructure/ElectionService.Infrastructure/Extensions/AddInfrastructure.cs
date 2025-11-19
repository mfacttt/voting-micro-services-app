using ElectionService.Domain.Repositories;
using ElectionService.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace ElectionService.Infrastructure.Extensions;

public static class AddInfrastructure
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<IElectionRepository, ElectionRepository>();
        services.AddScoped<ICandidateRepository, CandidateRepository>();
        
        return services;
    }
}