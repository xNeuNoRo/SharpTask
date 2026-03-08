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
    public List<NoteResponseDto> Notes { get; init; } = new();
}
