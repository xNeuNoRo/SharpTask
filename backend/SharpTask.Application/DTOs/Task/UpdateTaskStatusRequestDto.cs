using SharpTask.Domain.Enums;

namespace SharpTask.Application.DTOs.Task;

/// <summary>
/// DTO para la actualización del estado de una tarea
/// </summary>
public record UpdateTaskStatusRequestDto
{
    /// <summary>
    /// Nuevo estado de la tarea
    /// </summary>
    /// <example>Completed</example>
    public required TaskState Status { get; init; }
}
