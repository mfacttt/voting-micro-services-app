using Microsoft.Extensions.DependencyInjection;

namespace TallyService.Application.Extensions;

public static class AddApplication
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection service)
    {
        return service;
    }
}