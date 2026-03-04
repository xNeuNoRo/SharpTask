// Clase interna para tener un solo diccionario global en la app que maneje los locks por archivo,
// evitando problemas de concurrencia al acceder a los archivos JSON desde diferentes repositorios o hilos.
using System.Collections.Concurrent;

namespace SharpTask.Infrastructure.Repositories.Base;

internal static class JsonFileLockManager
{
    public static readonly ConcurrentDictionary<string, SemaphoreSlim> FileLocks = new();
}
