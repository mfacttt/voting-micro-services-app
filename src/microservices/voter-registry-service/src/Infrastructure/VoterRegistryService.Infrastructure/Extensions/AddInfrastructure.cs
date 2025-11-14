using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VoterRegistryService.Domain.Repositories;
using VoterRegistryService.Persistence.Context;
using VoterRegistryService.Persistence.Repositories;

namespace VoterRegistryService.Infrastructure.Extensions;

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
        services.AddDbContext<VoterRegistryDbContext>(options => options.UseInMemoryDatabase("VoterRegistryDb"));

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IVoterRepository, VoterRepository>();

        return services;
    }
}