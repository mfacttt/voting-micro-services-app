using Microsoft.Extensions.DependencyInjection;
using VoteCastingService.Application.Services;

namespace VoteCastingService.Application.Extensions;

public static class AddApplication
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IVoteCastingService, Services.VoteCastingService>();
        return services;
    }
}