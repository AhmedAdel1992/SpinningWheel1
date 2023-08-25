using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Postgres;

// public class PostgresDbContext:AppDbContext
// {
//     public PostgresDbContext(DbContextOptions<AppDbContext> options) : base(options)
//     {
//     }
//
//     protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//     {
//         optionsBuilder.UseNpgsql(opt => opt.UseNodaTime());
//         base.OnConfiguring(optionsBuilder);
//     }
// }
