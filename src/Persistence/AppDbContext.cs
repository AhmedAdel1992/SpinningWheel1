using System;
using Auditing;
using Domain.Entities;
using Domain.Entities.Base;
using Microsoft.EntityFrameworkCore;
using SmartEnum.EFCore;

namespace Persistence;

public class AppDbContext : DbContext, IAuditDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.UseAuditLogging();
        if (this.Database.IsNpgsql())
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        builder.ConfigureSmartEnum();

        base.OnModelCreating(builder);
    }
    
    public DbSet<SpinningWheel> SpinningWheels { get; set; }
    public DbSet<Segment> Segments { get; set; }
    public DbSet<Reward> Rewards { get; set; }
    public DbSet<ExtraData> ExtraDatas { get; set; }



}