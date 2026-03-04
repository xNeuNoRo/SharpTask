// Clase interna para tener un solo diccionario global en la app que maneje los locks por archivo,
// evitando problemas de concurrencia al acceder a los archivos JSON desde diferentes repositorios o hilos.
using System.Collections.Concurrent;

namespace SharpTask.Infrastructure.Repositories.Base;

internal static class JsonFileLockManager
{
    private static readonly ConcurrentDictionary<string, SemaphoreSlim> FileLocks = new();

    // Obtiene el lock asociado a un archivo específico. Si no existe, lo crea y lo agrega al diccionario.
    // Encapsulado de esta forma para evitar modificaciones directas al diccionario global, y asi lo encapsulamos y controlamos el acceso a los locks de manera centralizada.
    public static SemaphoreSlim GetOrCreateLock(string filePath)
    {
        // Si ya existe un lock para el archivo, lo retornamos. Si no, creamos uno nuevo y lo agregamos al diccionario.
        return FileLocks.GetOrAdd(filePath, _ => new SemaphoreSlim(1, 1));
    }
}
