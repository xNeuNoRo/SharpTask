using SharpTask.Application.Interfaces.Repositories.Base;
using SharpTask.Domain.Entities;

namespace SharpTask.Application.Interfaces.Repositories;

/// <summary>
/// Interfaz para el repositorio de notas que extiende la funcionalidad 
/// del repositorio base con operaciones específicas para las notas.
/// </summary>
public interface INoteRepository : IBaseRepository<NoteItem>
{
    Task<IEnumerable<NoteItem>> GetNotesByTaskIdAsync(Guid taskId);
    Task<bool> DeleteNotesByTaskIdAsync(Guid taskId);
}
