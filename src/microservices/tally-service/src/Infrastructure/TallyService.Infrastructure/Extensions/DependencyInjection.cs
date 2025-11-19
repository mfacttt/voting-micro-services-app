using Microsoft.Extensions.DependencyInjection;

namespace TallyService.Infrastructure.Extensions;

public static class AddInfrastructure
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection service)
    {
        return service;
    }
}