using SharpTask.Application.DTOs.Task;

namespace SharpTask.Application.Interfaces.Services;

/// <remarks>
/// Interfaz para el servicio de comandos de tareas (aplicando CQRS) que define los metodos
/// para modificar el estado de la aplicacion relacionado con las tareas, como crear, actualizar o eliminar tareas.
/// Este servicio se enfoca exclusivamente en las operaciones de escritura
/// </remarks>
public interface ITaskCommandService
{
    /// <summary>
    /// Crea una nueva tarea en el sistema con los datos proporcionados en el DTO de solicitud.
    /// </summary>
    /// <param name="request">Los datos para crear la tarea</param>
    /// <returns>La tarea creada</returns>
    Task<TaskResponseDto> CreateTaskAsync(CreateTaskRequestDto request);

    /// <summary>
    /// Actualiza una tarea existente por su ID utilizando los datos proporcionados en el DTO de solicitud.
    /// </summary>
    /// <param name="id">El identificador único de la tarea</param>
    /// <param name="request">Los datos para actualizar la tarea</param>
    /// <returns>La tarea actualizada o null si no se encuentra</returns>
    Task<TaskResponseDto?> UpdateTaskAsync(Guid id, UpdateTaskRequestDto request);

    /// <summary>
    /// Actualiza el estado de una tarea existente por su ID utilizando los datos proporcionados en el DTO de solicitud.
    /// </summary>
    /// <param name="id">El identificador único de la tarea</param>
    /// <param name="request">Los datos para actualizar el estado de la tarea</param>
    /// <returns>La tarea actualizada o null si no se encuentra</returns>
    Task<TaskResponseDto?> UpdateTaskStatusAsync(Guid id, UpdateTaskStatusRequestDto request);

    /// <summary>
    /// Elimina una tarea por su ID, eliminando también todas las notas asociadas a esa tarea.
    /// </summary>
    /// <param name="id">El identificador único de la tarea</param>
    /// <returns>True si la tarea fue eliminada, false en caso contrario</returns>
    Task<bool> DeleteTaskAsync(Guid id);
}
