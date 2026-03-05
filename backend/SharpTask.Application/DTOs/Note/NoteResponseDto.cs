namespace SharpTask.Application.DTOs.Note;

/// <summary>
/// DTO para la respuesta de una nota
/// </summary>
public record NoteResponseDto
{
    /// <summary>
    /// Identificador único de la nota
    /// </summary>
    /// <example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
    public Guid Id { get; init; }

    /// <summary>
    /// Identificador de la tarea a la que pertenece la nota
    /// </summary>
    /// <example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
    public Guid TaskId { get; init; }

    /// <summary>
    /// Contenido de la nota
    /// </summary>
    /// <example>Esta es una nota de ejemplo.</example>
    public required string Content { get; init; }

    /// <summary>
    /// Fecha y hora de creación de la nota en formato UTC
    /// </summary>
    /// <example>2026-06-01T12:00:00Z</example>
    public DateTime CreatedAt { get; init; }

    /// <summary>
    /// Fecha y hora de la última actualización de la nota en formato UTC
    /// </summary>
    /// <example>2026-06-01T12:00:00Z</example>
    public DateTime UpdatedAt { get; init; }
}
