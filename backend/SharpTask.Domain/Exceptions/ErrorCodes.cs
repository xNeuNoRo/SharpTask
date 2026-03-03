namespace SharpTask.Domain.Exceptions;

public static class ErrorCodes
{
    // Errores de tareas
    public const string TaskNotFound = "TASK_NOT_FOUND";

    // Errores de notas
    public const string NoteNotFound = "NOTE_NOT_FOUND";

    // Errores generales
    public const string BadRequest = "BAD_REQUEST";
    public const string Unauthorized = "UNAUTHORIZED";
    public const string Forbbiden = "FORBIDDEN";
    public const string NotFound = "NOT_FOUND";
    public const string ResourceConflict = "RESOURCE_CONFLICT";
    public const string InternalError = "INTERNAL_SERVER_ERROR";
    public const string ValidationError = "VALIDATION_ERROR";
}
