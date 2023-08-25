using Auditing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence;
using Persistence.DependencyInjection;

namespace MSSql.DependencyInjection;

public static class PersistenceExtension
{
    public static IServiceCollection AddMsSqlDependency(this IServiceCollection services,
        IConfiguration configuration)
    {
        var database = configuration.GetSection("Database").Get<string>();
        if (database != "MSSQL") return services;
        var connection = configuration.GetConnectionString("MSSqlConnection");
        services
            .AddDbContext<AppDbContext>((sp, opt) =>
                opt.UseSqlServer(connection,
                        b => b.MigrationsAssembly(typeof(PersistenceExtension).Assembly.FullName))
                    .AddAuditInterceptor(sp)
            );

        services.AddHostedService<ApplyMigrationHost>();
        return services;
    }
}