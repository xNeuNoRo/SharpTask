using SharpTask.Domain.Enums;

namespace SharpTask.Application.DTOs.Task;

/// <summary>
/// DTO para el cambio de estado de una tarea
/// </summary>
public record TaskChangeDto
{
    /// <summary>
    /// Estado de la tarea después del cambio
    /// </summary>
    /// <example>Completed</example>
    public TaskState Status { get; init; }

    /// <summary>
    /// Fecha y hora del cambio de estado en formato UTC
    /// </summary>
    /// <example>2026-06-01T12:00:00Z</example>
    public DateTime ChangedAt { get; init; }
}
