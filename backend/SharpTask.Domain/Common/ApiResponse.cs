namespace App.Entities;

public class ApiResponse<T>
{
    // Ok indica si la respuesta fue exitosa o no
    public bool Ok { get; set; }

    // Data contiene la informacion de la respuesta en caso de exito, o null si hubo un error
    public T? Data { get; set; }

    // Error contiene la informacion del error en caso de que Ok sea false, o null si la respuesta fue exitosa
    public object? Error { get; set; }

    // Constructor para respuestas exitosas
    public ApiResponse(T data)
    {
        Ok = true;
        Data = data;
    }

    // Constructor para respuestas de error
    public ApiResponse(string code, string message)
    {
        Ok = false;
        Error = new { code, message };
    }
}
