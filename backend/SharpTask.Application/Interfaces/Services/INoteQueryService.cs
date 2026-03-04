using SharpTask.Application.DTOs.Note;

namespace SharpTask.Application.Interfaces.Services;

/// <summary>
/// Interfaz para el servicio de consultas de notas (aplicando CQRS) que define los metodos
/// para obtener informacion relacionada con las notas sin modificar el estado de la app.
/// </summary>
public interface INoteQueryService
{
    Task<IEnumerable<NoteResponseDto>> GetNotesByTaskIdAsync(Guid taskId);
    Task<NoteResponseDto?> GetNoteByIdAsync(Guid id);
}
