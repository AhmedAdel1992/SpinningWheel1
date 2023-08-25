using System.Reflection;
using Application.Exceptions;
using FastEndpoints;
using LanguageExt.Common;
using MediatR;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Ardalis.SmartEnum;

namespace Wallet.Api.Endpoints.Base;


public class MyEndpoint<TRequest> : Endpoint<TRequest, object> where TRequest : notnull, new()
{
    protected Task SendResultAsync<T>(Result<T> response, int statusCode = 200,
        CancellationToken cancellation = default)
    {
        this.MatchResponse(HttpContext, response, statusCode, cancellation);
        return Task.CompletedTask;
    }


    protected async Task SendResultAsync<T>(T response, int statusCode = 200,
        CancellationToken cancellation = default) => await
        HttpContext.Response.SendAsync(response, statusCode, cancellation: cancellation);
}
public class MyEndpoint<TRequest, TResponse> : Endpoint<TRequest, TResponse> where TRequest : notnull
{
    public readonly IMediator _mediator;

    public MyEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }


    public override async Task HandleAsync(TRequest req, CancellationToken ct)
    {
        await HandleRequestAsync(req, ct);
    }

    public virtual async Task HandleRequestAsync(TRequest req, CancellationToken ct)
    {
        var result = (Result<TResponse>)await _mediator.Send(req, ct);
        await SendResultAsync(result, cancellation: ct);
    }

    protected Task SendResultAsync(Result<TResponse> response, int statusCode = 200,
        CancellationToken cancellation = default)
    {
        this.MatchResponse(HttpContext, response, statusCode, cancellation);
        ;
        return Task.CompletedTask;
    }
}

public static class MyEndpointExtension
{
    public static void MatchResponse<T>(this BaseEndpoint endpoint, HttpContext context, Result<T> response,
        int statusCode, CancellationToken cancellation)
    {
        response.Match(
            Succ: async r =>
            {
                if (r is FileStream)
                {
                    var stream = r as FileStream;
                    await context.Response.SendStreamAsync(
                        stream: stream,
                        fileName: stream.Name,
                        fileLengthBytes: stream.Length, cancellation: cancellation);
                }
                else
                {
                    await
                        context.Response.SendAsync(r, statusCode, cancellation: cancellation);
                }
            }
            ,
            Fail: async e =>
            {
                if (e is ApiException apiException)
                    await context.Response.SendAsync(new ApiErrorResponse(apiException), (int)apiException.StatusCode,
                        cancellation: cancellation);
                else
                {
                    await context.Response.SendAsync(e.Message, StatusCodes.Status500InternalServerError,
                        cancellation: cancellation);
                }
            });
    }
}

public sealed class SmartEnumSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        var type = context.Type;
        if (!IsTypeDerivedFromGenericType(type, typeof(SmartEnum<>)) &&
            !IsTypeDerivedFromGenericType(type, typeof(SmartEnum<,>)))
        {
            return;
        }

        var enumValues = type.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy)
            .Select(d => d.Name);
        var openApiValues = new OpenApiArray();
        openApiValues.AddRange(enumValues.Select(d => new OpenApiString(d)));

        // See https://swagger.io/docs/specification/data-models/enums/
        schema.Type = "string";
        schema.Enum = openApiValues;
        schema.Properties = null;
    }

    private static bool IsTypeDerivedFromGenericType(Type typeToCheck, Type genericType)
    {
        while (true)
        {
            if (typeToCheck == typeof(object))
            {
                return false;
            }

            if (typeToCheck == null)
            {
                return false;
            }

            if (typeToCheck.IsGenericType && typeToCheck.GetGenericTypeDefinition() == genericType)
            {
                return true;
            }

            typeToCheck = typeToCheck.BaseType;
        }
    }
}