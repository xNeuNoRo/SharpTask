using Mapster;
using SharpTask.Application.DTOs.Note;
using SharpTask.Application.DTOs.Task;
using SharpTask.Application.Interfaces.Repositories;
using SharpTask.Application.Interfaces.Services;
using SharpTask.Domain.Enums;

namespace SharpTask.Application.Services;

public class TaskQueryService : ITaskQueryService
{
    private readonly ITaskRepository _taskRepo;
    private readonly INoteRepository _noteRepo;

    /// <remarks>
    /// Constructor del servicio de consultas de tareas que recibe una instancia del repositorio de tareas
    /// </remarks>
    /// <param name="taskRepo">La instancia del repositorio de tareas.</param>
    /// <param name="noteRepo">La instancia del repositorio de notas.</param>
    public TaskQueryService(ITaskRepository taskRepo, INoteRepository noteRepo)
    {
        _taskRepo = taskRepo;
        _noteRepo = noteRepo;
    }

    /// <remarks>
    /// Obtiene todas las tareas de la base de datos
    /// y las mapea a DTOs de respuesta para ser consumidos por el frontend.
    /// </remarks>
    /// <returns>Una lista de DTOs de respuesta de tareas.</returns>
    public async Task<IEnumerable<TaskResponseDto>> GetAllTasksAsync()
    {
        var tasks = await _taskRepo.GetAllAsync();
        return tasks.Adapt<IEnumerable<TaskResponseDto>>();
    }

    /// <remarks>
    /// Obtiene una tarea por su ID, mapeándola a un DTO de detalle de tarea para proporcionar 
    /// información completa de la tarea, incluyendo sus notas relacionadas, para ser consumido por el frontend.
    /// </remarks>
    /// <param name="id">El ID de la tarea a obtener.</param>
    /// <returns>Un DTO de detalle de tarea o null si no se encuentra.</returns>
    public async Task<TaskDetailResponseDto?> GetTaskByIdAsync(Guid id)
    {
        // Obtiene la tarea por su ID
        var task = await _taskRepo.GetByIdAsync(id);

        // Si la tarea no existe, retorna null
        if (task == null)
            return null;

        // Si la tarea existe, obtiene las notas relacionadas a esta tarea
        var notes = await _noteRepo.GetNotesByTaskIdAsync(id);

        // Mapea la tarea y sus notas a un DTO de detalle de tarea para ser consumido por el frontend
        var response = task.Adapt<TaskDetailResponseDto>() with
        {
            Notes = notes.Adapt<List<NoteResponseDto>>(),
        };

        // Retorna el DTO de detalle de tarea con las notas relacionadas
        return response;
    }

    /// <remarks>
    /// Obtiene tareas filtradas por su estado, mapeándolas a DTOs de respuesta de tareas para ser consumidos por el frontend.
    /// </remarks>
    /// <param name="status">El estado de las tareas a obtener.</param>
    /// <returns>Una lista de DTOs de respuesta de tareas.</returns>
    public async Task<IEnumerable<TaskResponseDto>> GetTasksByStatusAsync(TaskState status)
    {
        var tasks = await _taskRepo.GetTasksByStatusAsync(status);
        return tasks.Adapt<IEnumerable<TaskResponseDto>>();
    }

    /// <remarks>
    /// Busca tareas por una palabra clave, mapeándolas a DTOs de respuesta de tareas para ser consumidos por el frontend.
    /// </remarks>
    /// <param name="keyword">La palabra clave para buscar tareas.</param>
    /// <returns>Una lista de DTOs de respuesta de tareas.</returns>
    public async Task<IEnumerable<TaskResponseDto>> SearchTasksAsync(string keyword)
    {
        var tasks = await _taskRepo.SearchTasksAsync(keyword);
        return tasks.Adapt<IEnumerable<TaskResponseDto>>();
    }
}
