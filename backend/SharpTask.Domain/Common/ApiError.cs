namespace SharpTask.Domain.Common;

/// <summary>
/// Clase que representa un error en la API, con un código de error,
/// un mensaje descriptivo y opcionalmente detalles adicionales sobre el error,
/// que pueden ser utilizados para proporcionar información más específica sobre el error ocurrido.
/// </summary>
public class ApiError
{
    /// <summary>
    /// Código de error
    /// </summary>
    /// <example>TASK_NOT_FOUND</example>
    public string Code { get; init; }

    /// <summary>
    /// Mensaje descriptivo del error
    /// </summary>
    /// <example>No se encontró la tarea con el ID especificado.</example>
    public string Message { get; init; }

    /// <summary>
    /// Detalles adicionales del error
    /// </summary>
    /// <example>{ "TaskId": "El ID de la tarea proporcionado no es válido." }</example>
    public object? Details { get; init; }

    /// <remarks>
    /// Constructor para crear un objeto ApiError,
    /// recibe un código de error y un mensaje, y los asigna a las propiedades correspondientes.
    /// </remarks>
    /// <param name="code">código de error</param>
    /// <param name="message">mensaje de error</param>
    public ApiError(string code, string message)
    {
        Code = code;
        Message = message;
    }

    /// <remarks>
    /// Constructor para crear un objeto ApiError con detalles adicionales,
    /// recibe un código de error, un mensaje y un objeto con detalles adicionales sobre el error,
    /// y los asigna a las propiedades correspondientes.
    /// </remarks>
    /// <param name="code">código de error</param>
    /// <param name="message">mensaje de error</param>
    /// <param name="details">detalles adicionales sobre el error</param>
    public ApiError(string code, string message, object details)
    {
        Code = code;
        Message = message;
        Details = details;
    }
}
