using SharpTask.Application.DTOs.Note;

namespace SharpTask.Application.Interfaces.Services;

public interface INoteCommandService
{
    Task<NoteResponseDto> CreateNoteAsync(Guid taskId, CreateNoteRequestDto request);
    Task<NoteResponseDto?> UpdateNoteAsync(Guid id, UpdateNoteRequestDto request);
    Task<bool> DeleteNoteAsync(Guid id);
}
