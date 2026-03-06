using SharpTask.Application.DTOs.Task;
using SharpTask.Domain.Enums;

namespace SharpTask.Application.Interfaces.Services;

/// <remarks>
/// Interfaz para el servicio de consultas de tareas (aplicando CQRS) que define los metodos
/// para obtener informacion relacionada con las tareas sin modificar el estado de la app.
/// </remarks>
public interface ITaskQueryService
{
    /// <summary>
    /// Obtiene todas las tareas disponibles en el sistema
    /// </summary>
    /// <returns>Una lista de tareas</returns>
    Task<IEnumerable<TaskResponseDto>> GetAllTasksAsync();

    /// <summary>
    /// Obtiene una tarea por su identificador único (GUID) incluyendo sus notas asociadas
    /// </summary>
    /// <param name="id">El identificador único de la tarea</param>
    /// <returns>La tarea encontrada o null si no se encuentra</returns>
    Task<TaskDetailResponseDto?> GetTaskByIdAsync(Guid id);

    /// <summary>
    /// Obtiene todas las tareas que tienen un estado específico, como "Pending", "InProgress" o "Completed"
    /// </summary>
    /// <param name="status">El estado de las tareas a obtener</param>
    /// <returns>Una lista de tareas con el estado especificado</returns>
    Task<IEnumerable<TaskResponseDto>> GetTasksByStatusAsync(TaskState status);

    /// <summary>
    /// Busca tareas por una palabra clave en el título o la descripción, devolviendo las tareas que coinciden con la búsqueda
    /// </summary>
    /// <param name="keyword">La palabra clave para buscar tareas</param>
    /// <returns>Una lista de tareas que coinciden con la búsqueda</returns>
    Task<IEnumerable<TaskResponseDto>> SearchTasksAsync(string keyword);
}
