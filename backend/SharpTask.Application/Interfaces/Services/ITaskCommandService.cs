using SharpTask.Application.DTOs.Task;

namespace SharpTask.Application.Interfaces.Services;

/// <remarks>
/// Interfaz para el servicio de comandos de tareas (aplicando CQRS) que define los metodos
/// para modificar el estado de la aplicacion relacionado con las tareas, como crear, actualizar o eliminar tareas.
/// Este servicio se enfoca exclusivamente en las operaciones de escritura
/// </remarks>
public interface ITaskCommandService
{
    Task<TaskResponseDto> CreateTaskAsync(CreateTaskRequestDto request);
    Task<TaskResponseDto?> UpdateTaskAsync(Guid id, UpdateTaskRequestDto request);
    Task<TaskResponseDto?> UpdateTaskStatusAsync(Guid id, UpdateTaskStatusRequestDto request);
    Task<bool> DeleteTaskAsync(Guid id);
}
