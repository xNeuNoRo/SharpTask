using SharpTask.Application.DTOs.Task;
using SharpTask.Domain.Enums;

namespace SharpTask.Application.Interfaces.Services;

/// <remarks>
/// Interfaz para el servicio de consultas de tareas (aplicando CQRS) que define los metodos
/// para obtener informacion relacionada con las tareas sin modificar el estado de la app.
/// </remarks>
public interface ITaskQueryService
{
    Task<IEnumerable<TaskResponseDto>> GetAllTasksAsync();
    Task<TaskDetailResponseDto?> GetTaskByIdAsync(Guid id);
    Task<IEnumerable<TaskResponseDto>> GetTasksByStatusAsync(TaskState status);
    Task<IEnumerable<TaskResponseDto>> SearchTasksAsync(string keyword);
}
