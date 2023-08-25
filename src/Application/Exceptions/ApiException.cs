using System.Net;

namespace Application.Exceptions;

public class ApiErrorResponse
{
    public ApiErrorResponse()
    {
        
    }
    public ApiErrorResponse(ApiException exception)
    {
        ErrorCode = exception.ErrorCode.ToString();
        ErrorMessage = exception.ErrorMessage;
        Details = exception.Details;
        StatusCode = exception.StatusCode;
        Errors = exception.Errors;
        if(exception.Errors is null || !exception.Errors.Any())
            Errors = new List<ErrorApi>
            {
                new(exception.ErrorCode.ToString(),exception.ErrorMessage)
            };
        
    }

    public string ErrorCode { get; set; }
    public string ErrorMessage { get; set; }
    public string Details { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public IEnumerable<ErrorApi> Errors { get; set; }
}

public class ApiException : ApplicationException
{
    private readonly ApiErrorType _apiError;

    public ApiException()
    {
        
    }
    public ApiException(string message) : this(message, null)
    {
    }

    public ApiException(string message, Exception exception) : this(message, exception, ApiErrorType.InternalError)
    {
    }

    public ApiException(ApiErrorType apiError) : this("", null, apiError)
    {
    }

    public ApiException(ApiErrorType apiError, string message) : this(message, null, apiError)
    {
    }

    public ApiException(ApiErrorType apiError, Exception ex) : this(ex?.Message, ex, apiError)
    {
    }

    public ApiException(string message, Exception exception, ApiErrorType apiError) : base(message, exception)
    {
        _apiError = apiError;
        SetException(message, exception);
    }

    public ApiException(string message, Exception exception, ApiErrorType apiError, IEnumerable<ErrorApi> errors) :
        base(message, exception)
    {
        _apiError = apiError;
        Errors = errors;
        SetException(message, exception);
    }

    public ApiException(ApiErrorType apiError, IEnumerable<ErrorApi> errors)
        : base(null, null)
    {
        _apiError = apiError;
        Errors = errors;
        SetException(null, null);
    }

    private void SetException(string message, Exception exception)
    {
        ErrorCode = _apiError.Value;
        string _message = "";
        if (!string.IsNullOrEmpty(message))
        {
            _message = message + "->";
        }

        if (!string.IsNullOrWhiteSpace(_apiError.Message))
            _message += _apiError.Message;
        else
            _message += Message;


        _message = _message.TrimEnd("->".ToCharArray());
        ErrorMessage = _message;

        exception = new Exception(_message, exception);

        StatusCode = _apiError.StatusCode;
        Details = UnwrapException(exception);
    }

    private string UnwrapException(Exception exception)
    {
        string _details = exception?.Message;
        while (exception?.InnerException != null)
        {
            _details += "\r\n" + exception.Message;
            exception = exception.InnerException;
        }

        return _details;
    }

    public HttpStatusCode StatusCode { get; private set; }
    public int ErrorCode { get; private set; }
    public string ErrorMessage { get; private set; }
    public string Details { get; private set; }
    public IEnumerable<ErrorApi> Errors { get; set; }
}