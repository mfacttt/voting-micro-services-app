using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VoteCastingService.Domain.Repositories;
using VoteCastingService.Infrastructure.Clients;
using VoteCastingService.Persistence.Context;
using VoteCastingService.Persistence.Repositories;

namespace VoteCastingService.Infrastructure.Extensions;

public static class AddInfrastructure
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPersistence(configuration);
        services.AddRepositories();
        return services;
    }

    private static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<VoteDbContext>(options =>
            options.UseNpgsql(connectionString));

        return services;
    }
    
    private static IServiceCollection AddClients(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IEligibilityClient, EligibilityClient>();
        services.AddScoped<IElectionStatusClient, ElectionStatusClient>();

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IVoteRepository, VoteRepository>();

        return services;
    }
}