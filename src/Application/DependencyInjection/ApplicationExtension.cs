using Application.Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application.DependencyInjection;

public static class ApplicationExtension
{
    public static IServiceCollection AddApplicationDependency(this IServiceCollection services,
        IConfiguration configuration)
    {

        services
            .AddMediatR(new []{ApplicationProj.Assembly, DomainProj.Assembly})
            .AddScoped(typeof(IPipelineBehavior<,>),typeof(ExceptionPipelineBehavior<,>))
            .AddScoped(typeof(IPipelineBehavior<,>),typeof(ValidationPipelineBehavior<,>))
            .AddValidatorsFromAssembly(typeof(ApplicationExtension).Assembly);
        
        return services;
    }
}