using Microsoft.Extensions.DependencyInjection;
using VoterRegistryService.Application.Services;

namespace VoterRegistryService.Application.Extensions;

public static class AddApplication
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IVoterService, VoterService>();

        return services;
    }
}