using SharpTask.Domain.Enums;

namespace SharpTask.Application.DTOs.Task;

/// <summary>
/// DTO para la respuesta de una tarea, incluyendo su historial de cambios de estado
/// </summary>
public record TaskResponseDto
{
    /// <summary>
    /// Identificador único de la tarea
    /// </summary>
    /// <example>d290f1ee-6c54-4bb6-9d8c-1e1e4a9c0c00</example>
    public Guid Id { get; init; }

    /// <summary>
    /// Título de la tarea
    /// </summary>
    /// <example>Comprar leche</example>
    public required string Title { get; init; }

    /// <summary>
    /// Descripción de la tarea
    /// </summary>
    /// <example>Comprar leche en el supermercado</example>
    public string? Description { get; init; }

    /// <summary>
    /// Estado de la tarea
    /// </summary>
    /// <example>Pending</example>
    public TaskState Status { get; init; }

    /// <summary>
    /// Lista de cambios de estado de la tarea
    /// </summary>
    public List<TaskChangeDto> Changes { get; init; } = new();

    /// <summary>
    /// Fecha y hora de creación de la tarea en formato UTC
    /// </summary>
    /// <example>2026-06-01T12:00:00Z</example>
    public DateTime CreatedAt { get; init; }

    /// <summary>
    /// Fecha y hora de actualización de la tarea en formato UTC
    /// </summary>
    /// <example>2026-06-01T12:00:00Z</example>
    public DateTime UpdatedAt { get; init; }
}
