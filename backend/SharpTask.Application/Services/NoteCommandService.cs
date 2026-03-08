using Mapster;
using SharpTask.Application.DTOs.Note;
using SharpTask.Application.Interfaces.Repositories;
using SharpTask.Application.Interfaces.Services;
using SharpTask.Domain.Entities;
using SharpTask.Domain.Exceptions;
using SharpTask.Domain.Interfaces;

namespace SharpTask.Application.Services;

/// <summary>
/// Servicio de comandos para las notas,
/// encargado de manejar la lógica de negocio relacionada con la creación,
/// actualización y eliminación de notas asociadas a tareas.
/// </summary>
public class NoteCommandService : INoteCommandService
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly INoteRepository _noteRepo;
    private readonly ITaskRepository _taskRepo;

    /// <remarks>
    /// Constructor del servicio de comandos de notas que recibe una instancia del repositorio de notas
    /// para acceder a los datos de las notas y proporcionar la funcionalidad de comando relacionada con las notas.
    /// </remarks>
    /// <param name="noteRepo">La instancia del repositorio de notas.</param>
    /// <param name="taskRepo">La instancia del repositorio de tareas.</param>
    /// <param name="dateTimeProvider">La instancia del proveedor de fechas y horas.</param>
    public NoteCommandService(
        INoteRepository noteRepo,
        ITaskRepository taskRepo,
        IDateTimeProvider dateTimeProvider
    )
    {
        _noteRepo = noteRepo;
        _taskRepo = taskRepo;
        _dateTimeProvider = dateTimeProvider;
    }

    /// <remarks>
    /// Crea una nueva nota asociada a una tarea específica utilizando los datos proporcionados en el DTO de solicitud.
    /// Mapea la nota creada a un DTO de respuesta para ser consumido por el frontend y lo devuelve.
    /// </remarks>
    /// <param name="taskId">El ID de la tarea a la que se asociará la nota.</param>
    /// <param name="request">El DTO de solicitud con los datos de la nota a crear.</param>
    /// <returns>El DTO de respuesta con los datos de la nota creada.</returns>
    public async Task<NoteResponseDto> CreateNoteAsync(Guid taskId, CreateNoteRequestDto request)
    {
        // Verificamos si la tarea a la que se intenta asociar la nota existe antes de crear la nota
        var exists = await _taskRepo.ExistsAsync(taskId);

        // Si la tarea no existe, lanzamos una excepción de tipo AppException
        // con un mensaje de error indicando que la tarea no existe
        if (!exists)
        {
            throw AppException.NotFound(
                "La tarea a la que se intenta asociar la nota no existe.",
                ErrorCodes.TaskNotFound
            );
        }

        // Obtenemos la hora actual para establecer la fecha de creación de la nota
        var currentTime = _dateTimeProvider.UtcNow;

        // Creamos una nueva instancia de NoteItem utilizando los datos del DTO de solicitud y la hora actual
        var newNote = new NoteItem(taskId, request.Content, currentTime);

        // Agregamos la nueva nota al repositorio y obtenemos la nota creada con su ID generado
        var createdNote = await _noteRepo.AddAsync(newNote);

        // Mapeamos la nota creada a un DTO de respuesta para ser consumido por el frontend y lo devolvemos
        return createdNote.Adapt<NoteResponseDto>();
    }

    /// <remarks>
    /// Actualiza una nota existente utilizando los datos proporcionados en el DTO de solicitud.
    /// Verifica si la nota existe antes de intentar actualizarla y devuelve null si no se puede actualizar.
    /// Mapea la nota actualizada a un DTO de respuesta para ser consumido por el frontend y lo devuelve.
    /// </remarks>
    /// <param name="taskId">El ID de la tarea a la que pertenece la nota.</param>
    /// <param name="id">El ID de la nota a actualizar.</param>
    /// <param name="request">El DTO de solicitud con los datos de la nota a actualizar.</param>
    /// <returns>El DTO de respuesta con los datos de la nota actualizada o null si no se puede actualizar.</returns>
    public async Task<NoteResponseDto?> UpdateNoteAsync(
        Guid taskId,
        Guid id,
        UpdateNoteRequestDto request
    )
    {
        // Obtenemos la nota existente por su ID para verificar si existe antes de intentar actualizarla
        var existingNote = await _noteRepo.GetByIdAsync(id);

        // Verificamos que la nota exista y que esté asociada a la tarea especificada antes de intentar actualizarla
        if (existingNote is null || existingNote.TaskId != taskId)
        {
            return null;
        }

        // Mapeamos los datos del DTO de solicitud a la nota existente
        // para actualizar sus propiedades con los nuevos valores proporcionados
        request.Adapt(existingNote);

        // Actualizamos la fecha de actualización de la nota a la hora actual utilizando el proveedor de fechas y horas
        existingNote.UpdatedAt = _dateTimeProvider.UtcNow;

        // Actualizamos la nota en el repositorio y obtenemos la nota actualizada
        var updatedNote = await _noteRepo.UpdateAsync(existingNote);

        // Mapeamos la nota actualizada a un DTO de respuesta para ser consumido por el frontend y lo devolvemos
        return updatedNote?.Adapt<NoteResponseDto>();
    }

    /// <remarks>
    /// Elimina una nota por su ID utilizando el repositorio de notas.
    /// </remarks>
    /// <param name="taskId">El ID de la tarea a la que pertenece la nota.</param>
    /// <param name="id">El ID de la nota a eliminar.</param>
    /// <returns>True si la nota existe y se elimino correctamente, false en caso contrario.</returns>
    public async Task<bool> DeleteNoteAsync(Guid taskId, Guid id)
    {
        // Obtenemos la nota existente por su ID para verificar si existe antes de intentar eliminarla
        var existingNote = await _noteRepo.GetByIdAsync(id);

        // Verificamos que la nota exista y que esté asociada a la tarea especificada antes de intentar eliminarla
        if (existingNote is null || existingNote.TaskId != taskId)
        {
            return false;
        }

        // Eliminamos la nota utilizando el repositorio de notas y devolvemos el resultado de la operación
        return await _noteRepo.DeleteAsync(id);
    }
}
