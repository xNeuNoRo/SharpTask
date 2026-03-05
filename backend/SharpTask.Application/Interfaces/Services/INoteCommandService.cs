using SharpTask.Application.DTOs.Note;

namespace SharpTask.Application.Interfaces.Services;

/// <summary>
/// Interfaz para el servicio de escrituras relacionados con las notas, incluyendo creación, actualización y eliminación
/// </summary>
public interface INoteCommandService
{
    /// <summary>
    /// Crea una nueva nota asociada a una tarea específica
    /// </summary>
    /// <param name="taskId">El identificador único de la tarea</param>
    /// <param name="request">Los datos para crear la nota</param>
    /// <returns>La nota creada</returns>
    Task<NoteResponseDto> CreateNoteAsync(Guid taskId, CreateNoteRequestDto request);

    /// <summary>
    /// Actualiza una nota existente por su ID
    /// </summary>
    /// <param name="id">El identificador único de la nota</param>
    /// <param name="request">Los datos para actualizar la nota</param>
    /// <returns>La nota actualizada o null si no se encuentra</returns>
    Task<NoteResponseDto?> UpdateNoteAsync(Guid id, UpdateNoteRequestDto request);

    /// <summary>
    /// Elimina una nota por su ID
    /// </summary>
    /// <param name="id">El identificador único de la nota</param>
    /// <returns>true si se elimina correctamente, false en caso contrario</returns>
    Task<bool> DeleteNoteAsync(Guid id);
}
