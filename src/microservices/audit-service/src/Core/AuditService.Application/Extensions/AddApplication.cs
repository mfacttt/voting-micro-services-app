using AuditService.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AuditService.Application.Extensions;

public static class AddApplication
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAuditLogService, AuditLogService>();

        return services;
    }
}