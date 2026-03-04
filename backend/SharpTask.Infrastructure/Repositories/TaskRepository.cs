using SharpTask.Application.Interfaces.Repositories;
using SharpTask.Domain.Entities;
using SharpTask.Domain.Entities.Tasks;
using SharpTask.Domain.Enums;
using SharpTask.Infrastructure.Repositories.Base;

namespace SharpTask.Infrastructure.Repositories;

public class TaskRepository : JsonBaseRepo<TaskItem>, ITaskRepository
{
    private readonly INoteRepository _noteRepository;

    /// <summary>
    /// Constructor del repositorio de tareas que recibe la ruta del archivo JSON y una instancia del repositorio de notas para manejar las relaciones entre tareas y notas.
    /// </summary>
    /// <param name="filePath">La ruta del archivo JSON donde se almacenan las tareas.</param>
    /// <param name="noteRepository">La instancia del repositorio de notas.</param>
    public TaskRepository(string filePath, INoteRepository noteRepository)
        : base(filePath)
    {
        _noteRepository = noteRepository;
    }

    // =================================
    // Implementacion del CRUD basico para TaskItem
    // que es la implementacion directa de IBaseRepository<TaskItem>
    // =================================

    /// <summary>
    /// Obtiene todas las tareas almacenadas en el repositorio.
    /// </summary>
    /// <returns>Una lista de objetos TaskItem.</returns>
    public async Task<IEnumerable<TaskItem>> GetAllAsync() => await base.LoadAsync();

    /// <summary>
    /// Obtiene una tarea por su identificador único (ID).
    /// </summary>
    /// <param name="id">El identificador único de la tarea.</param>
    /// <returns>La tarea encontrada o null si no existe.</returns>
    public async Task<TaskItem?> GetByIdAsync(Guid id) => await base.FindAsync(x => x.Id == id);

    /// <summary>
    /// Verifica si una tarea con el ID especificado existe en el repositorio.
    /// </summary>
    /// <param name="id">El identificador único de la tarea.</param>
    /// <returns>True si la tarea existe, false en caso contrario.</returns>
    public async Task<bool> ExistsAsync(Guid id) => (await GetByIdAsync(id)) != null;

    /// <summary>
    /// Agrega una nueva tarea al JSON.
    /// </summary>
    /// <param name="task">La tarea a agregar.</param>
    /// <returns>La tarea agregada.</returns>
    public async Task<TaskItem> AddAsync(TaskItem task)
    {
        await base.AppendAsync(task);
        return task;
    }

    /// <summary>
    /// Actualiza una tarea existente en el JSON.
    /// </summary>
    /// <param name="task">La tarea actualizada.</param>
    /// <returns>La tarea actualizada si se actualizó correctamente, o null si no se encontró o no se pudo actualizar.</returns>
    public async Task<TaskItem?> UpdateAsync(TaskItem task)
    {
        var updatedTask = await base.UpdateAsync(x => x.Id == task.Id, task);
        return updatedTask ? task : null;
    }

    /// <summary>
    /// Elimina una tarea por su ID del JSON.
    /// </summary>
    /// <param name="id">El identificador único de la tarea a eliminar.</param>
    /// <returns>True si la tarea fue eliminada, false en caso contrario.</returns>
    public async Task<bool> DeleteAsync(Guid id) => await base.DeleteAsync(x => x.Id == id);

    // =================================
    // Implementacion de las operaciones especificas
    // definidas en ITaskRepository para TaskItem
    // =================================

    /// <summary>
    /// Obtiene una tarea por su ID, incluyendo sus notas relacionadas.
    /// </summary>
    /// <param name="id">El identificador único de la tarea.</param>
    /// <returns>La tarea encontrada con sus notas incluidas o null si no existe.</returns>
    public async Task<TaskItem?> GetTaskWithNotesAsync(Guid id)
    {
        // Primero obtenemos la tarea básica sin las notas
        var task = await base.FindAsync(x => x.Id == id);

        // Si la tarea no existe, retornamos null
        if (task is null)
            return null;

        // Luego obtenemos las notas relacionadas a esta tarea usando el repositorio de notas
        var notes = await _noteRepository.GetNotesByTaskIdAsync(id);

        // Asignamos las notas obtenidas a la propiedad Notes de la tarea
        task.Notes = notes.ToList();

        // Finalmente, retornamos la tarea con sus notas incluidas
        return task;
    }

    /// <summary>
    /// Obtiene todas las tareas que tienen un estado específico.
    /// </summary>
    /// <param name="status">El estado de las tareas a obtener.</param>
    /// <returns>Una lista de tareas con el estado especificado.</returns>
    public async Task<IEnumerable<TaskItem>> GetTasksByStatusAsync(TaskState status)
    {
        return await base.FindManyAsync(x => x.Status == status);
    }

    /// <summary>
    /// Busca tareas que contengan una palabra clave en su título o descripción.
    /// </summary>
    /// <param name="keyword">La palabra clave a buscar.</param>
    /// <returns>Una lista de tareas que coinciden con la palabra clave.</returns>
    public async Task<IEnumerable<TaskItem>> SearchTasksAsync(string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
            return await GetAllAsync();

        return await base.FindManyAsync(x =>
            x.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase)
            || (
                !string.IsNullOrEmpty(x.Description)
                && x.Description.Contains(keyword, StringComparison.OrdinalIgnoreCase)
            )
        );
    }
}
