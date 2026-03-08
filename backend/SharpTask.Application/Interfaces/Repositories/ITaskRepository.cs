using SharpTask.Application.Interfaces.Repositories.Base;
using SharpTask.Domain.Entities.Tasks;
using SharpTask.Domain.Enums;

namespace SharpTask.Application.Interfaces.Repositories;

/// <remarks>
/// Interfaz para el repositorio de tareas que extiende la funcionalidad
/// del repositorio base con operaciones específicas para las tareas.
/// </remarks>
public interface ITaskRepository : IBaseRepository<TaskItem>
{
    /// <summary>
    /// Obtiene todas las tareas que tienen un estado específico
    /// </summary>
    /// <param name="status">El estado de las tareas a obtener</param>
    /// <returns>Una lista de tareas con el estado especificado</returns>
    Task<IEnumerable<TaskItem>> GetTasksByStatusAsync(TaskState status);

    /// <summary>
    /// Busca tareas por una palabra clave en el título o la descripción
    /// </summary>
    /// <param name="keyword">La palabra clave para buscar</param>
    /// <returns>Una lista de tareas que coinciden con la palabra clave</returns>
    Task<IEnumerable<TaskItem>> SearchTasksAsync(string keyword);
}
