using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharpTask.Application.Interfaces.Repositories;
using SharpTask.Infrastructure.Repositories;

namespace SharpTask.Infrastructure;

public static class DependencyInjection
{
    /// <summary>
    /// Extensión para configurar los servicios de infraestructura en el contenedor de dependencias.
    /// </summary>
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

        // Registramos el repositorio de notas
        services.AddScoped<INoteRepository>(provider =>
        {
            return new NoteRepository(notesFilePath);
        });

        // Registramos el repositorio de tareas, inyectando el repositorio de notas para manejar las relaciones entre tareas y notas
        services.AddScoped<ITaskRepository>(provider =>
        {
            var noteRepo = provider.GetRequiredService<INoteRepository>();
            return new TaskRepository(tasksFilePath, noteRepo);
        });

        // Retornamos el contenedor de servicios actualizado
        return services;
    }
}
