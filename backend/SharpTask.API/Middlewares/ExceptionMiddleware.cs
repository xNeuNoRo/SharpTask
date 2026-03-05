using System.Net;
using System.Text.Json;
using SharpTask.Domain.Common;
using SharpTask.Domain.Exceptions;

namespace SharpTask.API.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    // Constructor del middleware, recibe el siguiente delegado en la cadena de middlewares y un logger para registrar errores
    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    // Este método se ejecuta para cada solicitud HTTP que pasa por el middleware
    public async Task InvokeAsync(HttpContext context)
    {
        // Intentamos procesar la solicitud normalmente
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            // Si ocurre una excepción, la registramos en el log para debug posterior
            _logger.LogError(
                ex,
                "Ha ocurrido un error inesperado en el pipeline de procesamiento de la solicitud."
            );

            // Luego, manejamos la excepción y construimos una respuesta HTTP adecuada para el cliente
            await HandleExceptionAsync(context, ex, _logger);
        }
    }

    // Este método maneja la excepción y construye una respuesta HTTP adecuada
    private static Task HandleExceptionAsync(
        HttpContext context,
        Exception exception,
        ILogger logger
    )
    {
        // Configuramos la respuesta HTTP para devolver un JSON
        context.Response.ContentType = "application/json";

        // Por defecto, asumimos un error 500 - Internal Server Error
        var statusCode = (int)HttpStatusCode.InternalServerError;
        var message = "Ha ocurrido un error inesperado en el servidor.";
        string? errorCode = ErrorCodes.InternalError;

        // Pero, si la excepción es una AppException, podemos extraer el código de error y el mensaje específico
        if (exception is AppException appEx)
        {
            // En este caso, usamos el código de estado, mensaje y código de error definidos en la AppException
            statusCode = appEx.StatusCode;
            message = appEx.Message;
            errorCode = appEx.Code;

            // Loggeamos la excepción de negocio o validación como una advertencia,
            // ya que es un error esperado que puede ocurrir durante el uso normal de la aplicación
            logger.LogWarning(
                "Validación/Lógica de Negocio fallida: {Message} (Code: {Code}, Status: {Status})",
                message,
                errorCode,
                statusCode
            );
        }
        else
        {
            // Si es cualquier otra excepción no controlada, la loggeamos como un error crítico,
            // ya que indica un problema inesperado que no se previo en el servidor
            logger.LogError(
                exception,
                "Ha ocurrido un error crítico no controlado en el servidor."
            );
        }

        // Establecemos el código de estado HTTP en la respuesta
        context.Response.StatusCode = statusCode;

        // Creamos un objeto de respuesta con el formato definido en ApiResponse, indicando que la respuesta no fue exitosa y proporcionando el mensaje de error
        var response = new ApiResponse<object>(errorCode, message);

        // Configuramos la serializacion del JSON para usar camelCase en los nombres de las propiedades
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        // Serializamos el objeto de respuesta a JSON y lo escribimos en la respuesta HTTP
        return context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
    }
}
