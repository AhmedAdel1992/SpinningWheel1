using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ValidationEngine.Interfaces;
using ValidationEngine.Services;

namespace ValidationEngine.DependencyInjection;

public static class ValidationEngineExtension
{
    public static IServiceCollection AddValidationEngineDependency(this IServiceCollection services,
        Assembly[] assemblies = default)
    {
        services
            .AddScoped<IOperatorService, OperatorService>()
            .AddScoped<IValidationBuilder, ValidationBuilder>()
            .AddScoped<IRuleService, RuleService>()
            .AddSingleton<IEntityService, EntityService>()
            ;
        // get all classes implements ISetting interface

// get all properties of this class 
        return services;
    }

    public static WebApplication AddValidationEngineEndpoints(this WebApplication app ,Assembly[] assemblies = default)
    {

        var entityService =  app.Services.GetService<IEntityService>();

        var types = assemblies == null
            ? Assembly.GetEntryAssembly()?.GetTypes()
            : assemblies.SelectMany(s => s.GetTypes());
        var settings = types?.Where(p => typeof(ISetting).IsAssignableFrom(p) && !p.IsInterface && !p.IsAbstract)
            .ToList();
        if (entityService is not null)
            settings?.ForEach(x =>
                {
                    entityService.AddFields(x.Name,
                        x.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly));
                }
            );
        
        app.MapGet("/api/operators", (IOperatorService service) => service.GetOperators());
        app.MapGet("/api/operators/{type}", (IOperatorService service, string type) => service.GetOperators(type));
        app.MapGet("/api/types", (IOperatorService service) => service.GetTypes());
        app.MapGet("/api/entities", (IEntityService service) => service.GetEntities());
        app.MapGet("/api/entities/{entity}", (IEntityService service, string entity) => service.GetFields(entity));
        return app;
    }
}