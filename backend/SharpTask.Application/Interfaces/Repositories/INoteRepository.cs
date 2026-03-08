using SharpTask.Application.Interfaces.Repositories.Base;
using SharpTask.Domain.Entities;

namespace SharpTask.Application.Interfaces.Repositories;

/// <remarks>
/// Interfaz para el repositorio de notas que extiende la funcionalidad 
/// del repositorio base con operaciones específicas para las notas.
/// </remarks>
public interface INoteRepository : IBaseRepository<NoteItem>
{
    /// <summary>
    /// Obtiene todas las notas asociadas a una tarea específica por su ID
    /// </summary>
    /// <param name="taskId">El identificador único de la tarea</param>
    /// <returns>Una lista de notas asociadas a la tarea</returns>
    Task<IEnumerable<NoteItem>> GetNotesByTaskIdAsync(Guid taskId);

    /// <summary>
    /// Elimina todas las notas asociadas a una tarea específica por su ID
    /// </summary>
    /// <param name="taskId">El identificador único de la tarea</param>
    /// <returns>True si las notas fueron eliminadas, false en caso contrario</returns>
    Task<bool> DeleteNotesByTaskIdAsync(Guid taskId);
}
