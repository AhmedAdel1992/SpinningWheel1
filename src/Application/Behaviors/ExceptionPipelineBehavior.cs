using Application.Exceptions;
using Common.Logging;
using MediatR;

namespace Application.Behaviors;

public class ExceptionPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILoggerAdapter<TRequest> _logger;

    public ExceptionPipelineBehavior(ILoggerAdapter<TRequest> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Exception in {RequestName}", typeof(TRequest).Name);
            return CreateValidationResult<TResponse>(e);
        }
    }

    private TResult CreateValidationResult<TResult>(Exception exception)
    {
        var result = (TResult)Activator.CreateInstance(typeof(TResult), new ApiException(ApiErrorType.InternalError, exception))!;
        return result;
    }
}