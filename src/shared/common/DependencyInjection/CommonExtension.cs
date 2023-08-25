using System.Configuration;
using Common.Files;
using Common.Logging;
using Common.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Common.DependencyInjection;

public static class CommonExtension
{
    public static IServiceCollection AddCommon(this IServiceCollection services,IConfiguration configuration)
    {
        
        services
            //Register Options
            .Configure<GeneratorOptions>(configuration.GetSection(nameof(GeneratorOptions)))
            .Configure<RabbitMqOption>(configuration.GetSection(nameof(RabbitMqOption)))
            
            ;
        return services;
    }   
    public static IHostBuilder UseCommon(this IHostBuilder services)
    {
        
        services
            //Using serilog
            .UseSerilog((context, configuration) =>
            {
                var env = context.HostingEnvironment.EnvironmentName;
                var config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", true)
                    .AddJsonFile($"appsettings.{env}.json", true)
                    .Build();
                 configuration
                    .ReadFrom.Configuration(config)
                    .Enrich.FromLogContext();
                Log.Logger.Information("current env is {env}", env);
            })
            .ConfigureAppConfiguration((context, builder) =>
            {
                builder.AddEnvironmentVariables(prefix: "App_");
            })
            ;
        return services;
    }
}