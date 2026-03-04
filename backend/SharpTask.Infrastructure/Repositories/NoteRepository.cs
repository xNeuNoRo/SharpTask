using SharpTask.Application.Interfaces.Repositories;
using SharpTask.Domain.Entities;
using SharpTask.Infrastructure.Repositories.Base;

namespace SharpTask.Infrastructure.Repositories;

public class NoteRepository : JsonBaseRepo<NoteItem>, INoteRepository
{
    /// <summary>
    /// Constructor del repositorio de notas que recibe la ruta del archivo JSON donde se almacenan las notas. Este constructor llama al constructor base de JsonBaseRepo para inicializar el repositorio con la ruta proporcionada.
    /// </summary>
    /// <param name="filePath">La ruta del archivo JSON donde se almacenan las notas.</param>
    public NoteRepository(string filePath)
        : base(filePath) { }

    // =================================
    // Implementacion del CRUD basico para NoteItem
    // que es la implementacion directa de IBaseRepository<TaskItem>
    // =================================

    /// <summary>
    /// Obtiene todas las notas almacenadas en el repositorio.
    /// </summary>
    /// <returns>Una lista de objetos NoteItem.</returns>
    public async Task<IEnumerable<NoteItem>> GetAllAsync() => await base.LoadAsync();

    /// <summary>
    /// Obtiene una nota por su identificador único (ID).
    /// </summary>
    /// <param name="id">El identificador único de la nota.</param>
    /// <returns>La nota encontrada o null si no existe.</returns>
    public async Task<NoteItem?> GetByIdAsync(Guid id) => await base.FindAsync(x => x.Id == id);

    /// <summary>
    /// Verifica si una nota con el ID especificado existe en el repositorio.
    /// </summary>
    /// <param name="id">El identificador único de la nota.</param>
    /// <returns>True si la nota existe, false en caso contrario.</returns>
    public async Task<bool> ExistsAsync(Guid id) => (await GetByIdAsync(id)) != null;

    /// <summary>
    /// Agrega una nueva nota al JSON.
    /// </summary>
    /// <param name="note">La nota a agregar.</param>
    /// <returns>La nota agregada.</returns>
    public async Task<NoteItem> AddAsync(NoteItem note)
    {
        await base.AppendAsync(note);
        return note;
    }

    /// <summary>
    /// Actualiza una nota existente en el JSON.
    /// </summary>
    /// <param name="note">La nota a actualizar.</param>
    /// <returns>La nota actualizada.</returns>
    public async Task<NoteItem> UpdateAsync(NoteItem note)
    {
        await base.UpdateAsync(x => x.Id == note.Id, note);
        return note;
    }

    /// <summary>
    /// Elimina una nota por su ID del JSON.
    /// </summary>
    /// <param name="id">El identificador único de la nota a eliminar.</param>
    /// <returns>True si la nota fue eliminada, false en caso contrario.</returns>
    public async Task<bool> DeleteAsync(Guid id) => await base.DeleteAsync(x => x.Id == id);

    // =================================
    // Implementacion de metodos adicionales para NoteItem
    // que son las implementaciones directas de los metodos
    // definidos en INoteRepository para NoteItem
    // =================================

    /// <summary>
    /// Obtiene todas las notas asociadas a una tarea específica por su ID. Este método busca en el repositorio todas las notas que tengan un TaskId que coincida con el ID de la tarea proporcionada y devuelve una lista de esas notas.
    /// </summary>
    /// <param name="taskId">El identificador único de la tarea.</param>
    /// <returns>Una lista de objetos NoteItem asociados a la tarea.</returns>
    public async Task<IEnumerable<NoteItem>> GetNotesByTaskIdAsync(Guid taskId)
    {
        return await base.FindManyAsync(x => x.TaskId == taskId);
    }

    /// <summary>
    /// Elimina todas las notas asociadas a una tarea específica por su ID. Devuelve true si todas las notas fueron eliminadas correctamente, o false si alguna nota no pudo ser eliminada.
    /// </summary>
    /// <param name="taskId">El identificador único de la tarea.</param>
    /// <returns>True si todas las notas fueron eliminadas correctamente, false en caso contrario.</returns>
    public async Task<bool> DeleteNotesByTaskIdAsync(Guid taskId)
    {
        return await base.DeleteAsync(x => x.TaskId == taskId);
    }
}
