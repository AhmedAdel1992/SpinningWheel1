using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Auditing;

public static class ModelBuilderExtension
{
    
    public static DbContextOptionsBuilder AddAuditInterceptor(this DbContextOptionsBuilder builder,IServiceProvider sp)
    {
        var dataInterceptor = sp.GetService<TrackDataInterceptor>();
        builder.AddInterceptors(dataInterceptor);
       
        return builder;
    }
    
    public static IServiceCollection AddAuditLogging(this IServiceCollection services)
    {
        services.AddScoped<TrackDataInterceptor>(sp =>
        {
            var auditService = sp.GetService<IBaseAuditService>();
            return new TrackDataInterceptor(auditService);
        });
        return services;
    }
    
    private static readonly JsonSerializerOptions _serializeOptions=new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };
    public static ModelBuilder UseAuditLogging(this ModelBuilder modelBuilder)
    {
        // iterate all entities and find all fields of specific type, then set value converter for them

        // modelBuilder.Model.AddEntityType(typeof(UserActionLog));
        var builder = modelBuilder.Entity<UserActionLog>();
        
        builder.ToTable("UserActionLogs");
        builder.HasKey(p=>p.Id);
        builder.Property(p => p.Id).ValueGeneratedOnAdd();
        builder.HasIndex(p => p.ActionTime);
        builder.HasIndex(p => p.TableName);
        builder.HasIndex(p => p.PrimaryKey);
        
        builder.Property(p=>p.OldValues)
            .HasConversion(
                ol=>JsonSerializer.Serialize(ol, _serializeOptions),
                ol=>JsonSerializer.Deserialize<Dictionary<string,object>>(ol,_serializeOptions)
            );
        builder.Property(p=>p.NewValues)
            .HasConversion(
                ol=>JsonSerializer.Serialize(ol, _serializeOptions),
                ol=>JsonSerializer.Deserialize<Dictionary<string,object>>(ol,_serializeOptions)
            );
        builder.Property(p=>p.ChangedColumns)
            .HasConversion(
                ol=>JsonSerializer.Serialize(ol, _serializeOptions),
                ol=>JsonSerializer.Deserialize<List<string>>(ol,_serializeOptions)!
            );
        return modelBuilder;
    }
}