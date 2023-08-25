using Application.Exceptions;
using FluentValidation;
using MediatR;

namespace Application.Behaviors;

public class ValidationPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationPipelineBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
            return await next();

        var errors = _validators
            .Select(v => v.ValidateAsync(request, cancellationToken).Result)
            .SelectMany(v => v.Errors)
            .Where(v => v is not null)
            .Select(f => new ErrorApi(f.PropertyName, f.ErrorMessage))
            .Distinct()
            .ToArray();

        if (errors.Any())
        {
            return CreateValidationResult<TResponse>(errors);
        }
        
        return await next();
        
    }

    private TResult CreateValidationResult<TResult>(ErrorApi[] errors)
    {
        var result = (TResult)Activator.CreateInstance(typeof(TResult), new object[] { new ApiException(ApiErrorType.ValidationError, errors) })!;
        return result;
    }
}