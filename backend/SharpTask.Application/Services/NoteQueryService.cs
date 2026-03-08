using Mapster;
using SharpTask.Application.DTOs.Note;
using SharpTask.Application.Interfaces.Repositories;
using SharpTask.Application.Interfaces.Services;
using SharpTask.Domain.Exceptions;

namespace SharpTask.Application.Services;

/// <summary>
/// Servicio de consultas para las notas,
/// encargado de manejar la lógica de negocio relacionada con la obtención de notas,
/// incluyendo la recuperación de todas las notas, la obtención de una nota por su ID
/// y la obtención de notas asociadas a una tarea específica.
/// </summary>
public class NoteQueryService : INoteQueryService
{
    private readonly INoteRepository _noteRepo;
    private readonly ITaskRepository _taskRepo;

    /// <remarks>
    /// Constructor del servicio de consultas de notas que recibe una instancia del
    /// repositorio de notas para acceder a los datos de las notas y proporcionar la
    /// funcionalidad de consulta relacionada con las notas.
    /// </remarks>
    /// <param name="noteRepo">La instancia del repositorio de notas.</param>
    /// <param name="taskRepo">La instancia del repositorio de tareas.</param>
    public NoteQueryService(INoteRepository noteRepo, ITaskRepository taskRepo)
    {
        _noteRepo = noteRepo;
        _taskRepo = taskRepo;
    }

    /// <remarks>
    /// Obtiene todas las notas de la base de datos y las mapea a DTOs de respuesta para ser consumidos por el frontend.
    /// </remarks>
    /// <returns>Una tarea que representa la operación asincrónica.</returns>
    public async Task<IEnumerable<NoteResponseDto>> GetAllNotesAsync()
    {
        var notes = await _noteRepo.GetAllAsync();
        return notes.Adapt<IEnumerable<NoteResponseDto>>();
    }

    /// <remarks>
    /// Obtiene una nota por su ID, mapeándola a un DTO de respuesta de nota para proporcionar
    /// la información necesaria al frontend. Si la nota no se encuentra, devuelve null.
    /// </remarks>
    /// <param name="taskId">El ID de la tarea a la que pertenece la nota.</param>
    /// <param name="id">El ID de la nota a obtener.</param>
    /// <returns>
    /// Una tarea que representa la operación asincrónica,
    /// con un DTO de respuesta de nota o null si no se encuentra.
    /// </returns>
    public async Task<NoteResponseDto?> GetNoteByIdAsync(Guid taskId, Guid id)
    {
        var note = await _noteRepo.GetByIdAsync(id);

        // Verificamos que la nota exista y que esté asociada a la tarea especificada antes de mapearla a un DTO de respuesta
        if (note is null || note.TaskId != taskId)
        {
            return null;
        }

        return note.Adapt<NoteResponseDto>();
    }

    /// <remarks>
    /// Obtiene las notas asociadas a una tarea específica por su ID,
    /// mapeándolas a DTOs de respuesta de notas para ser consumidos por el frontend.
    /// Si no se encuentran notas para la tarea, devuelve una lista vacía.
    /// </remarks>
    /// <param name="taskId">El ID de la tarea para la cual obtener notas.</param>
    /// <returns>Una lista de DTOs de respuesta de notas.</returns>
    public async Task<IEnumerable<NoteResponseDto>> GetNotesByTaskIdAsync(Guid taskId)
    {
        if (!await ExistsTask(taskId))
        {
            throw AppException.NotFound(
                "La tarea especificada no existe.",
                ErrorCodes.TaskNotFound
            );
        }

        // Obtenemos las notas asociadas a la tarea utilizando el repositorio de notas
        // y las mapeamos a DTOs de respuesta para ser consumidos por el frontend
        var notes = await _noteRepo.GetNotesByTaskIdAsync(taskId);
        return notes.Adapt<IEnumerable<NoteResponseDto>>();
    }

    /// <summary>
    /// Valida si una tarea existe en la base de datos por su ID, utilizando el repositorio de tareas,
    /// esta validación es necesaria para asegurarnos de que las notas que se intentan obtener
    /// están asociadas a una tarea válida, si la tarea no existe, la validación falla y se devuelve false,
    /// lo que permite manejar adecuadamente el caso en el que se intentan obtener notas para
    /// una tarea que no existe, evitando así errores
    /// y proporcionando una respuesta clara al frontend sobre la inexistencia de la tarea.
    /// </summary>
    /// <param name="taskId">El ID de la tarea a validar.</param>
    /// <returns>Un valor booleano que indica si la tarea existe.</returns>
    private async Task<bool> ExistsTask(Guid taskId)
    {
        return await _taskRepo.ExistsAsync(taskId);
    }
}
