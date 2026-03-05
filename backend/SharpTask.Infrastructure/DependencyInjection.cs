using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharpTask.Application.Interfaces.Repositories;
using SharpTask.Domain.Interfaces;
using SharpTask.Infrastructure.Providers;
using SharpTask.Infrastructure.Repositories;

namespace SharpTask.Infrastructure;

public static class DependencyInjection
{
    /// <remarks>
    /// Extensión para configurar los servicios de infraestructura en el contenedor de dependencias.
    /// </remarks>
    /// <param name="services">El contenedor de servicios de la aplicación.</param>
    /// <param name="configuration">La configuración de la aplicación.</param>
    /// <returns>El contenedor de servicios actualizado.</returns>
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        // Configuración de rutas para los archivos JSON de tareas y notas
        var basePath = AppContext.BaseDirectory;

        // Obtenemos las rutas relativas de los archivos JSON desde la configuración, con valores predeterminados si no están configurados
        var tasksRelativePath = configuration["FileStorage:TasksFilePath"] ?? "Data/tasks.json";
        var notesRelativePath = configuration["FileStorage:NotesFilePath"] ?? "Data/notes.json";

        // Combinamos la ruta base con las rutas relativas para obtener las rutas completas de los archivos JSON
        var tasksFilePath = Path.Combine(basePath, tasksRelativePath);
        var notesFilePath = Path.Combine(basePath, notesRelativePath);

        // Registramos el proveedor de fecha y hora como un servicio singleton
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        // Registramos el repositorio de notas
        services.AddScoped<INoteRepository>(provider => new NoteRepository(notesFilePath));

        // Registramos el repositorio de tareas
        services.AddScoped<ITaskRepository>(provider => new TaskRepository(tasksFilePath));

        // Retornamos el contenedor de servicios actualizado
        return services;
    }
}
