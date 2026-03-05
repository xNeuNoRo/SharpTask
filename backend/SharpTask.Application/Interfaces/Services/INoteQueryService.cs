using SharpTask.Application.DTOs.Note;

namespace SharpTask.Application.Interfaces.Services;

/// <remarks>
/// Interfaz para el servicio de consultas de notas (aplicando CQRS) que define los metodos
/// para obtener informacion relacionada con las notas sin modificar el estado de la app.
/// </remarks>
public interface INoteQueryService
{
    Task<IEnumerable<NoteResponseDto>> GetAllNotesAsync();
    Task<NoteResponseDto?> GetNoteByIdAsync(Guid id);
    Task<IEnumerable<NoteResponseDto>> GetNotesByTaskIdAsync(Guid taskId);
}
