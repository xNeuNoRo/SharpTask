using SharpTask.Application.DTOs.Note;

namespace SharpTask.Application.DTOs.Task;

/// <summary>
/// DTO para la respuesta detallada de una tarea, incluyendo sus notas asociadas
/// </summary>
public record TaskDetailResponseDto : TaskResponseDto
{
    /// <summary>
    /// Lista de notas asociadas a la tarea
    /// </summary>
    /// <example>[
    ///   {
    ///     "id": "d290f1ee-6c54-4bb6-9d8c-1e1e4a9c0c00",
    ///    "content": "Esta es una nota de ejemplo."
    ///  }
    /// ]</example>
    public List<NoteResponseDto> Notes { get; init; } = new();
}
