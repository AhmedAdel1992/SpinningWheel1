using System.Text.Json;
using System.Text.Json.Serialization;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Wallet.Api;
using Wallet.Integration.Tests.Servers;
using Xunit;

namespace Wallet.Integration.Tests.Factories;

public class ApiFactory : WebApplicationFactory<IApiMarker>, IAsyncLifetime
{
    private readonly TestcontainerDatabase _mssqlContainer;
    private string DbProvider = "MSSQL";

    public ApiFactory()
    {
        _mssqlContainer = GetContainerBuilder();
    }

    TestcontainerDatabase GetContainerBuilder()
    {
        var containerName = $"wallet_service_test_{Guid.NewGuid()}";
        var password = "P@ssw0rd123";
        return DbProvider switch
        {
            "MSSQL" => new ContainerBuilder<MsSqlTestcontainer>()
                .WithName(containerName)
                .WithDatabase(new MsSqlTestcontainerConfiguration("mcr.microsoft.com/mssql/server:2022-latest")
                {
                    Password = password,
                    Database = containerName
                }).Build(),
            "Postgres" => new ContainerBuilder<PostgreSqlTestcontainer>()
                .WithName(containerName)
                .WithDatabase(new PostgreSqlTestcontainerConfiguration()
                {
                    Username = "admin",
                    Password = password,
                    Database = containerName
                }).Build(),
            _ => throw new NotSupportedException()
        };
    }
    
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureHostConfiguration((configBuilder) =>
        {
            // configBuilder.Sources.Clear();

            configBuilder.AddInMemoryCollection(new Dictionary<string, string>
            {
                {
                    "ConnectionStrings:MSSqlConnection",
                    $"{_mssqlContainer.ConnectionString};TrustServerCertificate=True"
                },
                {
                    "ASPNETCORE_ENVIRONMENT",
                    $"Development"
                },
                { "Database", DbProvider }
            }!);
        });
        return base.CreateHost(builder);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureLogging(logging => logging.ClearProviders());
        builder.ConfigureTestServices(services =>
        {
            services.Configure<JsonOptions>(options =>
            {
                options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            });
            services.AddMassTransitTestHarness(cfg => { });
        });
    }

    public async Task InitializeAsync()
    {
        if (_mssqlContainer is not null)
            await _mssqlContainer.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await _mssqlContainer.StopAsync();
        foreach (var factory in this.Factories)
            await factory.DisposeAsync();
    }
}