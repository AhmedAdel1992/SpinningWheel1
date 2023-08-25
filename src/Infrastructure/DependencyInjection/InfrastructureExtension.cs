using Auditing;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DependencyInjection;

public static class InfrastructureExtension
{
    public static IServiceCollection AddInfrastructureDependency(this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddScoped<IBaseAuditService,AuditService>()
            .AddMemoryCache();
        return services;
    }
}