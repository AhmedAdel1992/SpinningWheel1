using Microsoft.Extensions.DependencyInjection;

namespace Common.Extensions;

public static class RegisterOptionsExtension
{
    public static IServiceCollection RegisterOptions<TOptions>(this IServiceCollection services, string sectionName)
        where TOptions : class
    {
        // services.Configure<TOptions>(options => Configuration.GetSection(sectionName).Bind(options));
        
        return services;
    }
}