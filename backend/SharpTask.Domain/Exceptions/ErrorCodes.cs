namespace SharpTask.Domain.Exceptions;

public static class ErrorCodes
{
    // Errores de tareas
    public const string TaskNotFound = "TASK_NOT_FOUND";

    // Errores de notas
    public const string NoteNotFound = "NOTE_NOT_FOUND";

    // Errores generales
    public const string InternalError = "INTERNAL_SERVER_ERROR";
    public const string ValidationError = "VALIDATION_ERROR";
}
