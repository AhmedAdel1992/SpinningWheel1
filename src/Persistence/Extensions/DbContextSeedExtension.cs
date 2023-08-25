using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Persistence.Seeds.Base;

namespace Persistence.Extensions;

public static class DbContextSeedExtension
{

    public static async  Task ApplySeeds(this  AppDbContext context,IConfiguration configuration,CancellationToken cancellationToken=default)
    {
        var seedClasses = typeof(IEntitySeed).Assembly
            .GetTypes()
            .Where(t => t is { IsAbstract: false, IsInterface: false } && 
                        t.GetInterfaces().Contains(typeof(IEntitySeed)))
            .ToList();

        var seeds = seedClasses
            .Select(s => (IEntitySeed)Activator.CreateInstance(s)!)
            .ToList();
        foreach (var seed in seeds)
        {
            await seed.SeedAsync(context,configuration, cancellationToken);
        }
    }
}