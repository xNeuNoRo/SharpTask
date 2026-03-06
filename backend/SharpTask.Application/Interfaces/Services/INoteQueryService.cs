using SharpTask.Application.DTOs.Note;

namespace SharpTask.Application.Interfaces.Services;

/// <remarks>
/// Interfaz para el servicio de consultas de notas (aplicando CQRS) que define los metodos
/// para obtener informacion relacionada con las notas sin modificar el estado de la app.
/// </remarks>
public interface INoteQueryService
{
    /// <summary>
    /// Obtiene todas las notas disponibles en el sistema
    /// </summary>
    /// <returns>Una lista de todas las notas</returns>
    Task<IEnumerable<NoteResponseDto>> GetAllNotesAsync();

    /// <summary>
    /// Obtiene una nota por su identificador único (GUID)
    /// </summary>
    /// <param name="id">El identificador único de la nota</param>
    /// <returns>La nota encontrada o null si no se encuentra</returns>
    Task<NoteResponseDto?> GetNoteByIdAsync(Guid id);

    /// <summary>
    /// Obtiene todas las notas asociadas a una tarea específica
    /// </summary>
    /// <param name="taskId">El identificador único de la tarea</param>
    /// <returns>Una lista de notas asociadas a la tarea</returns>
    Task<IEnumerable<NoteResponseDto>> GetNotesByTaskIdAsync(Guid taskId);
}
