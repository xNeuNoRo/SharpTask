namespace App.Entities.Exceptions;

public class AppException : Exception
{
    public int StatusCode { get; set; }
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
    public static AppException BadRequest(string message, string code = "BAD_REQUEST") =>
        new(code, 400, message);

    // Error 401 - Unauthorized
    public static AppException Unauthorized(
        string message = "No autorizado",
        string code = "UNAUTHORIZED"
    ) => new(code, 401, message);

    // Error 403 - Forbidden
    public static AppException Forbidden(
        string message = "Acceso prohibido",
        string code = "FORBIDDEN"
    ) => new(code, 403, message);

    // Error 404 - Not Found
    public static AppException NotFound(string message, string code = "NOT_FOUND") =>
        new(code, 404, message);

    // Error 409 - Conflict
    public static AppException Conflict(string message, string code = "RESOURCE_CONFLICT") =>
        new(code, 409, message);

    // Error 500 - Internal Server Error
    public static AppException InternalServer(
        string message,
        string code = "INTERNAL_SERVER_ERROR"
    ) => new(code, 500, message);
}
