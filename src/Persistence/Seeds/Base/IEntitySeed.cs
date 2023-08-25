using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Persistence.Seeds.Base;

public interface IEntitySeed 
{
    Task SeedAsync(AppDbContext context,IConfiguration configuration, CancellationToken cancellationToken=default);

}