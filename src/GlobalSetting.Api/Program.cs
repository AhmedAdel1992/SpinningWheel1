using Application.DependencyInjection;
using Common.Extensions;
using FastEndpoints;
using FastEndpoints.Swagger;
using Wallet.Api.DependencyInjection;
using Wallet.Api.Endpoints.Base;

var assemblies = new[]
{
    typeof(Program).Assembly, PersistenceProj.Assembly, DomainProj.Assembly, ApplicationProj.Assembly,
    InfrastructureProj.Assembly, CommonProj.Assembly
};


var builder = WebApplication.CreateBuilder(args);
builder.Host.UseCommon();
builder.Services
    .AddMapster(assemblies)
    .AddAutoRegister(assemblies)
    .AddInfrastructureDependency(builder.Configuration)
    .AddApplicationDependency(builder.Configuration)
    .AddMsSqlDependency(builder.Configuration)
    .AddPersistenceDependency(builder.Configuration)
    .AddWalletDependency(builder.Configuration)
    .AddFastEndpoints(c => { })
    .AddEndpointsApiExplorer()
    .AddSwaggerDoc()
    .AddCors()
    .AddSwaggerGen(c => c.SchemaFilter<SmartEnumSchemaFilter>())
    ;
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

var app = builder.Build();
app
    .UseAuthentication()
    .UseAuthorization()
    .UseFastEndpoints(c => c.Endpoints.RoutePrefix = "api")
    .UseSwaggerGen()
    .UseCors(opt => opt.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.MapGet("/", () => "Hello World!");


app.Run();