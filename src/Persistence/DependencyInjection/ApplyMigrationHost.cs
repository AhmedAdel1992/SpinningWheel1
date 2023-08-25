using System.Threading;
using System.Threading.Tasks;
using Common.DependencyInjection.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Persistence.Extensions;

namespace Persistence.DependencyInjection;

public class ApplyMigrationHost : IHostedService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IConfiguration _configuration;

    public ApplyMigrationHost(IServiceScopeFactory scopeFactory,IConfiguration configuration)
    {
        _scopeFactory = scopeFactory;
        _configuration = configuration;
    }


    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var startups = scope.ServiceProvider.GetServices<IAfterMigrateStartup>();
        await context.Database.MigrateAsync(cancellationToken);
        await context.ApplySeeds(_configuration,cancellationToken);
        
        foreach (var startup in startups)
        {
            await startup.StartAsync(cancellationToken);
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
    }
}