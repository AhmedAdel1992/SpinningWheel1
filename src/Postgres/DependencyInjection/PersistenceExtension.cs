using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence;
using Persistence.DependencyInjection;

namespace Postgres.DependencyInjection;

public static class PersistenceExtension
{
    public static IServiceCollection AddPostgresDependency(this IServiceCollection services,
        IConfiguration configuration)
    {
        var database = configuration.GetSection("Database").Get<string>();
        if (database != "Postgres") return services;
        services
            .AddDbContext<AppDbContext>(opt =>
                opt.UseNpgsql(configuration.GetConnectionString("PostgresConnection"),
                    b =>
                    {
                        // b.UseNodaTime();
                        b.MigrationsAssembly(typeof(PersistenceExtension).Assembly.FullName);
                    }
                )
            );
        services.AddHostedService<ApplyMigrationHost>();

        return services;
    }
}