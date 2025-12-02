using ElectionService.Application.Services.CandidateService;
using ElectionService.Application.Services.ElectionService;
using Microsoft.Extensions.DependencyInjection;

namespace ElectionService.Application.Extensions;

public static class AddApplication
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IElectionService, Services.ElectionService.ElectionService>();
        services.AddScoped<ICandidateService, CandidateService>();

        return services;
    }
}