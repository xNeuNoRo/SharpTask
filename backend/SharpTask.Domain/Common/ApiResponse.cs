namespace SharpTask.Domain.Common;

public class ApiResponse<T>
{
    // Ok indica si la respuesta fue exitosa o no
    public bool Ok { get; private set; }

    // Data contiene la informacion de la respuesta en caso de exito, o null si hubo un error
    public T? Data { get; private set; }

    // Error contiene la informacion del error en caso de que Ok sea false, o null si la respuesta fue exitosa
    public ApiError? Error { get; private set; }

    /// <remarks>
    /// Constructor para respuestas exitosas,
    /// recibe los datos de la respuesta y establece Ok en true,
    /// Data con los datos proporcionados y Error en null.
    /// </remarks>
    /// <param name="data">datos de la respuesta</param>
    public ApiResponse(T data)
    {
        Ok = true;
        Data = data;
        Error = null;
    }

    /// <remarks>
    /// Constructor para respuestas de error,
    /// recibe un objeto ApiError y establece Ok en false,
    /// Data en null y Error con el objeto proporcionado.
    /// </remarks>
    /// <param name="error">información del error</param>
    public ApiResponse(ApiError error)
    {
        Ok = false;
        Data = default;
        Error = error;
    }

    /// <remarks>
    /// Constructor para respuestas de error,
    /// recibe un código de error y un mensaje,
    /// establece Ok en false, Data en null y Error con un nuevo objeto ApiError construido
    /// </remarks>
    /// <param name="code">código de error</param>
    /// <param name="message">mensaje de error</param>
    public ApiResponse(string code, string message)
    {
        Ok = false;
        Data = default;
        Error = new ApiError(code, message);
    }

    /// <remarks>
    /// Método estático de conveniencia para crear una respuesta exitosa,
    /// recibe los datos y devuelve un nuevo ApiResponse con esos datos.
    /// </remarks>
    /// <param name="data">datos de la respuesta</param>
    /// <returns>respuesta exitosa</returns>
    public static ApiResponse<T> Success(T data) => new ApiResponse<T>(data);

    /// <remarks>
    /// Método estático de conveniencia para crear una respuesta de error,
    /// recibe un objeto ApiError y devuelve un nuevo ApiResponse con ese error.
    /// </remarks>
    /// <param name="error">Un objeto de tipo ApiError ya construido con sus datos</param>
    /// <returns>respuesta de error</returns>
    public static ApiResponse<T> Failure(ApiError error) => new ApiResponse<T>(error);

    /// <remarks>
    /// Método estático de conveniencia para crear una respuesta de error,
    /// recibe un código de error y un mensaje, y devuelve un nuevo ApiResponse con esa
    /// </remarks>
    /// <param name="code">código de error</param>
    /// <param name="message">mensaje de error</param>
    /// <returns>respuesta de error</returns>
    public static ApiResponse<T> Failure(string code, string message) =>
        new ApiResponse<T>(code, message);
}
