using Microsoft.Extensions.Configuration;

namespace Common.Entities;

public interface IEntitySeed 
{
    Task SeedAsync<TContext>(TContext context,IConfiguration configuration, CancellationToken cancellationToken=default);

}