using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Persistence.Seeds.Base;

public abstract class EntitySeed<TEntity> : IEntitySeed
    where TEntity : class
{
    protected abstract Task ApplySeedsAsync(DbSet<TEntity> entity, List<TEntity> entities);
    protected abstract Task ApplySeedsAsync(DbSet<TEntity> entity);

    public async Task SeedAsync(AppDbContext context, IConfiguration configuration,
        CancellationToken cancellationToken = default)
    {
        var entitySet = context.Set<TEntity>();
        var entityName = typeof(TEntity).Name;
        var entitiesFromJson = configuration.GetSection(entityName).Get<TEntity[]>();
        
        if (entitiesFromJson is not null && entitiesFromJson.Any())
            await ApplySeedsAsync(entitySet, entitiesFromJson.ToList());

        await ApplySeedsAsync(entitySet);
        await context.SaveChangesAsync(cancellationToken);
    }
}