using System.Reflection;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Extensions;

public static class MapsterExtension
{
    public static IServiceCollection AddMapster(this IServiceCollection services,Assembly[] assemblies)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(assemblies);
        return services;
    }
}