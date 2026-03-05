namespace SharpTask.Domain.Common;

public class ApiError
{
    public string Code { get; init; }
    public string Message { get; init; }
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
