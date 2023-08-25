using System.Reflection;
using Common.DependencyInjection.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Common.DependencyInjection;

public static class AutoRegisterExtension
{
    public static IServiceCollection AddAutoRegister(this IServiceCollection services,Assembly[] assemblies)
    {
        services.Scan(scan => scan.AddTypeSourceSelector(assemblies));
        return services;
    }
    
    private static ITypeSourceSelector AddTypeSourceSelector(this ITypeSourceSelector scan,Assembly[] assemblies)
    {
        scan
            .FromAssemblies(assemblies)
            .AddClasses(classes => classes.AssignableTo<ITransient>())
            .AsImplementedInterfaces(i => i != typeof(INotRegistered))
            .WithTransientLifetime()
            .AddClasses(classes => classes.AssignableTo<IScoped>())
            .AsImplementedInterfaces(i =>i != typeof(INotRegistered))
            .WithScopedLifetime()
            .AddClasses(classes => classes.AssignableTo<ISingleton>())
            .AsImplementedInterfaces(i => i != typeof(INotRegistered))
            .WithSingletonLifetime()
            .AddClasses(classes => classes.AssignableTo<ISelfScoped>())
            .AsSelf()
            .WithScopedLifetime()
            .AddClasses(classes => classes.AssignableTo<ISelfSingleton>())
            .AsSelf()
            .WithSingletonLifetime()
            ;
        return scan;
    }
}