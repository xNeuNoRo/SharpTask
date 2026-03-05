using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SharpTask.Application.Interfaces.Services;
using SharpTask.Application.Services;

namespace SharpTask.Application;

public static class DependencyInjection
{
    /// <summary>
    /// Extensión para configurar los servicios de la capa de aplicación en el contenedor de dependencias.
    /// </summary>
    /// <param name="services">El contenedor de servicios de la aplicación</param>
    /// <returns>El contenedor de servicios actualizado.</returns>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Escanear el ensamblado actual en busca de clases que implementen AbstractValidator<T> 
        // y registrarlas automáticamente (esto es parte de FluentValidation)
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        // Registrar los servicios de query de tareas y notas
        services.AddScoped<ITaskQueryService, TaskQueryService>();
        services.AddScoped<INoteQueryService, NoteQueryService>();

        // Registrar los servicios de escritura de tareas y notas
        services.AddScoped<ITaskCommandService, TaskCommandService>();
        services.AddScoped<INoteCommandService, NoteCommandService>();

        // Retornamos el servicio actualizado
        return services;
    }
}
