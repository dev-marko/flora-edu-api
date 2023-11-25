using System.Net;

namespace FloraEdu.Domain.Exceptions;

public class ApiException : Exception
{
    public string ErrorCode { get; }

    public ApiException(string message, string errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }
}

public static class ApiExceptionExtension
{
    private static readonly Dictionary<string, HttpStatusCode> ApiExceptionErrorMessages = new()
    {
        { ErrorCodes.NotFound, HttpStatusCode.NotFound },
        { ErrorCodes.BadRequest, HttpStatusCode.BadRequest },
        { ErrorCodes.InternalServerError, HttpStatusCode.InternalServerError },
        { ErrorCodes.OperationFailed, HttpStatusCode.InternalServerError },

        { ErrorCodes.UserNotFound, HttpStatusCode.NotFound },
        { ErrorCodes.PasswordMismatch, HttpStatusCode.BadRequest }
    };

    public static HttpStatusCode GetStatusCode(string errorMessage)
    {
        foreach (var (key, value) in ApiExceptionErrorMessages)
        {
            if (errorMessage.Contains(key))
            {
                return value;
            }
        }

        return HttpStatusCode.InternalServerError;
    }
}
