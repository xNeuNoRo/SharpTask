using SharpTask.Application.Interfaces.Repositories;
using SharpTask.Domain.Entities;
using SharpTask.Domain.Entities.Tasks;
using SharpTask.Domain.Enums;
using SharpTask.Infrastructure.Repositories.Base;

namespace SharpTask.Infrastructure.Repositories;

/// <summary>
/// Repositorio concreto para manejar las operaciones CRUD y demas
/// de las tareas (TaskItem) utilizando un archivo JSON como almacenamiento.
/// </summary>
public class TaskRepository : JsonBaseRepo<TaskItem>, ITaskRepository
{
    /// <remarks>
    /// Constructor del repositorio de tareas que recibe la ruta del archivo JSON donde se almacenan las tareas. 
    /// Este constructor llama al constructor de la clase base JsonBaseRepo para inicializar el repositorio 
    /// con la ruta del archivo JSON proporcionada.
    /// </remarks>
    /// <param name="filePath">La ruta del archivo JSON donde se almacenan las tareas.</param>
    public TaskRepository(string filePath)
        : base(filePath) { }

    // =================================
    // Implementacion del CRUD basico para TaskItem
    // que es la implementacion directa de IBaseRepository<TaskItem>
    // =================================

    /// <remarks>
    /// Obtiene todas las tareas almacenadas en el repositorio.
    /// </remarks>
    /// <returns>Una lista de objetos TaskItem.</returns>
    public async Task<IEnumerable<TaskItem>> GetAllAsync() => await base.LoadAsync();

    /// <remarks>
    /// Obtiene una tarea por su identificador único (ID).
    /// </remarks>
    /// <param name="id">El identificador único de la tarea.</param>
    /// <returns>La tarea encontrada o null si no existe.</returns>
    public async Task<TaskItem?> GetByIdAsync(Guid id) => await base.FindAsync(x => x.Id == id);

    /// <remarks>
    /// Verifica si una tarea con el ID especificado existe en el repositorio.
    /// </remarks>
    /// <param name="id">El identificador único de la tarea.</param>
    /// <returns>True si la tarea existe, false en caso contrario.</returns>
    public async Task<bool> ExistsAsync(Guid id) => (await base.FindAsync(x => x.Id == id)) != null;

    /// <remarks>
    /// Agrega una nueva tarea al JSON.
    /// </remarks>
    /// <param name="task">La tarea a agregar.</param>
    /// <returns>La tarea agregada.</returns>
    public async Task<TaskItem> AddAsync(TaskItem task)
    {
        await base.AppendAsync(task);
        return task;
    }

    /// <remarks>
    /// Actualiza una tarea existente en el JSON.
    /// </remarks>
    /// <param name="task">La tarea actualizada.</param>
    /// <returns>La tarea actualizada si se actualizó correctamente, o null si no se encontró o no se pudo actualizar.</returns>
    public async Task<TaskItem?> UpdateAsync(TaskItem task)
    {
        var updatedTask = await base.UpdateAsync(x => x.Id == task.Id, task);
        return updatedTask ? task : null;
    }

    /// <remarks>
    /// Elimina una tarea del JSON por su ID
    /// </remarks>
    /// <param name="id">El identificador único de la tarea a eliminar.</param>
    /// <returns>True si la tarea fue eliminada, false en caso contrario.</returns>
    public async Task<bool> DeleteAsync(Guid id)
    {
        // Retorna el resultado de la eliminación de la tarea
        return await base.DeleteAsync(x => x.Id == id);
    }

    // =================================
    // Implementacion de las operaciones especificas
    // definidas en ITaskRepository para TaskItem
    // =================================

    /// <remarks>
    /// Obtiene todas las tareas que tienen un estado específico.
    /// </remarks>
    /// <param name="status">El estado de las tareas a obtener.</param>
    /// <returns>Una lista de tareas con el estado especificado.</returns>
    public async Task<IEnumerable<TaskItem>> GetTasksByStatusAsync(TaskState status)
    {
        return await base.FindManyAsync(x => x.Status == status);
    }

    /// <remarks>
    /// Busca tareas que contengan una palabra clave en su título o descripción.
    /// </remarks>
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
