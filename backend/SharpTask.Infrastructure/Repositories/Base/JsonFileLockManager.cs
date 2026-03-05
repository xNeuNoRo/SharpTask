using System.Collections.Concurrent;

namespace SharpTask.Infrastructure.Repositories.Base;

/// <summary>
/// Clase interna para manejar los locks de acceso a archivos JSON de manera centralizada,
/// utilizando un diccionario concurrente que asocia cada ruta de archivo con un SemaphoreSlim,
/// lo que permite controlar el acceso concurrente a los archivos JSON y evitar conflictos o corrupciones
/// al acceder a los archivos desde diferentes partes de la aplicación, proporcionando una forma segura
/// y eficiente de manejar los locks de archivos JSON en toda la aplicación.
/// </summary>
internal static class JsonFileLockManager
{
    /// <summary>
    /// Diccionario concurrente que asocia cada ruta de archivo JSON con un SemaphoreSlim,
    /// utilizado para controlar el acceso concurrente a los archivos JSON en la aplicación.
    /// </summary>
    private static readonly ConcurrentDictionary<string, SemaphoreSlim> FileLocks = new();

    /// <summary>
    /// Obtiene el lock asociado a un archivo específico. Si no existe, lo crea y lo agrega al diccionario.
    /// Encapsulado de esta forma para evitar modificaciones directas al diccionario global, y asi lo encapsulamos y controlamos el acceso a los locks de manera centralizada.
    /// </summary>
    /// <param name="filePath">La ruta del archivo para el cual obtener el lock.</param>
    /// <returns>El SemaphoreSlim asociado al archivo.</returns>
    public static SemaphoreSlim GetOrCreateLock(string filePath)
    {
        // Si ya existe un lock para el archivo, lo retornamos. Si no, creamos uno nuevo y lo agregamos al diccionario.
        return FileLocks.GetOrAdd(filePath, _ => new SemaphoreSlim(1, 1));
    }
}
