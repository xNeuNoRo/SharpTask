using Mapster;
using SharpTask.Application.DTOs.Note;
using SharpTask.Application.Interfaces.Repositories;
using SharpTask.Application.Interfaces.Services;
using SharpTask.Domain.Entities;

namespace SharpTask.Application.Services;

public class NoteQueryService : INoteQueryService
{
    private readonly INoteRepository _noteRepo;

    /// <summary>
    /// Constructor del servicio de consultas de notas que recibe una instancia del 
    /// repositorio de notas para acceder a los datos de las notas y proporcionar la 
    /// funcionalidad de consulta relacionada con las notas.
    /// </summary>
    /// <param name="noteRepo">La instancia del repositorio de notas.</param>
    public NoteQueryService(INoteRepository noteRepo)
    {
        _noteRepo = noteRepo;
    }

    /// <summary>
    /// Obtiene todas las notas de la base de datos y las mapea a DTOs de respuesta para ser consumidos por el frontend.
    /// </summary>
    /// <returns>Una tarea que representa la operación asincrónica.</returns>
    public async Task<IEnumerable<NoteResponseDto>> GetAllNotesAsync()
    {
        var notes = await _noteRepo.GetAllAsync();
        return notes.Adapt<IEnumerable<NoteResponseDto>>();
    }

    /// <summary>
    /// Obtiene una nota por su ID, mapeándola a un DTO de respuesta de nota para proporcionar
    /// la información necesaria al frontend. Si la nota no se encuentra, devuelve null.
    /// </summary>
    /// <param name="id">El ID de la nota a obtener.</param>
    /// <returns>
    /// Una tarea que representa la operación asincrónica,
    /// con un DTO de respuesta de nota o null si no se encuentra.
    /// </returns>
    public async Task<NoteResponseDto?> GetNoteByIdAsync(Guid id)
    {
        var note = await _noteRepo.GetByIdAsync(id);
        return note?.Adapt<NoteResponseDto>();
    }

    /// <summary>
    /// Obtiene las notas asociadas a una tarea específica por su ID,
    /// mapeándolas a DTOs de respuesta de notas para ser consumidos por el frontend.
    /// Si no se encuentran notas para la tarea, devuelve una lista vacía.
    /// </summary>
    /// <param name="taskId">El ID de la tarea para la cual obtener notas.</param>
    /// <returns>Una lista de DTOs de respuesta de notas.</returns>
    public async Task<IEnumerable<NoteResponseDto>> GetNotesByTaskIdAsync(Guid taskId)
    {
        var notes = await _noteRepo.GetNotesByTaskIdAsync(taskId);
        return notes.Adapt<IEnumerable<NoteResponseDto>>();
    }
}
