namespace FloraEdu.Domain.Exceptions;

public static class ErrorCodes
{
    public const string NotFound = nameof(NotFound);
    public const string BadRequest = nameof(BadRequest);
    public const string InternalServerError = nameof(InternalServerError);
    public const string OperationFailed = nameof(OperationFailed);

    public const string UserNotFound = nameof(UserNotFound);
    public const string UserExists = nameof(UserExists);
    public const string PasswordMismatch = nameof(PasswordMismatch);
}
