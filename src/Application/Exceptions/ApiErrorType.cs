using System.Net;
using Ardalis.SmartEnum;

namespace Application.Exceptions;

public class ApiErrorType : SmartEnum<ApiErrorType>
{
    public static readonly ApiErrorType NotFound = new ApiErrorType(nameof(NotFound), 1, HttpStatusCode.NotFound, "Record not found");
    public static readonly ApiErrorType ValidationError = new ApiErrorType(nameof(ValidationError), 2, HttpStatusCode.BadRequest, "Validation error");
    public static readonly ApiErrorType PnrHasNoDisruptionFlights = new ApiErrorType(nameof(PnrHasNoDisruptionFlights), 3, HttpStatusCode.BadRequest, "PNR has no disruption flights");
    
    public static readonly ApiErrorType InternalError = new ApiErrorType(nameof(InternalError), 100, HttpStatusCode.InternalServerError, "Internal error");
    public ApiErrorType(string name, int value,HttpStatusCode statusCode,string message) : base(name, value)
    {
        StatusCode = statusCode;
        Message = message;
    }
    public HttpStatusCode StatusCode { get; }
    public string Message { get; }
}