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
    Task<IEnumerable<TaskItem>> GetTasksByStatusAsync(TaskState status);
    Task<IEnumerable<TaskItem>> SearchTasksAsync(string keyword);
}
