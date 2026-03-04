using SharpTask.Application.DTOs.Note;

namespace SharpTask.Application.DTOs.Task;

public record TaskDetailResponseDto : TaskResponseDto
{
    public List<NoteResponseDto> Notes { get; init; } = new();
}
