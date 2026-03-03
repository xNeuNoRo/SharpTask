namespace SharpTask.Domain.Exceptions;

public class AppException : Exception
{
    public int StatusCode { get; }
    public string Code { get; }

    public AppException(string code, int statusCode, string message)
        : base(message)
    {
        Code = code;
        StatusCode = statusCode;
    }

    //*******************************************
    // Errores comunes predefinidos
    // *****************************************

    // Error 400 - Bad Request
    public static AppException BadRequest(string message, string code = ErrorCodes.BadRequest) =>
        new(code, 400, message);

    // Error 401 - Unauthorized
    public static AppException Unauthorized(
        string message = "No autorizado",
        string code = ErrorCodes.Unauthorized
    ) => new(code, 401, message);

    // Error 403 - Forbidden
    public static AppException Forbidden(
        string message = "Acceso prohibido",
        string code = ErrorCodes.Forbidden
    ) => new(code, 403, message);

    // Error 404 - Not Found
    public static AppException NotFound(string message, string code = ErrorCodes.NotFound) =>
        new(code, 404, message);

    // Error 409 - Conflict
    public static AppException Conflict(
        string message,
        string code = ErrorCodes.ResourceConflict
    ) => new(code, 409, message);

    // Error 500 - Internal Server Error
    public static AppException InternalServer(
        string message,
        string code = ErrorCodes.InternalError
    ) => new(code, 500, message);
}
