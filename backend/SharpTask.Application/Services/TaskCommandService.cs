using Mapster;
using SharpTask.Application.DTOs.Task;
using SharpTask.Application.Interfaces.Repositories;
using SharpTask.Application.Interfaces.Services;
using SharpTask.Domain.Entities.Tasks;
using SharpTask.Domain.Interfaces;

namespace SharpTask.Application.Services;

public class TaskCommandService : ITaskCommandService
{
    private readonly ITaskRepository _taskRepo;
    private readonly IDateTimeProvider _dateTimeProvider;

    /// <summary>
    /// Constructor del servicio de comandos de tareas que recibe una instancia del
    /// repositorio de tareas y un proveedor de fecha y hora para manejar las operaciones
    /// de escritura relacionadas con las tareas, como crear, actualizar o eliminar tareas.
    /// Este servicio se enfoca exclusivamente en las operaciones de escritura.
    /// </summary>
    /// <param name="taskRepo">La instancia del repositorio de tareas.</param>
    /// <param name="dateTimeProvider">La instancia del proveedor de fecha y hora.</param>
    public TaskCommandService(ITaskRepository taskRepo, IDateTimeProvider dateTimeProvider)
    {
        _taskRepo = taskRepo;
        _dateTimeProvider = dateTimeProvider;
    }

    /// <summary>
    /// Crea una nueva tarea en la base de datos utilizando los datos proporcionados en el DTO de solicitud.
    /// Mapea la tarea creada a un DTO de respuesta para ser consumido por el frontend y lo devuelve.
    /// </summary>
    /// <param name="request">El DTO de solicitud con los datos para crear la tarea.</param>
    /// <returns>El DTO de respuesta con los datos de la tarea creada.</returns>
    public async Task<TaskResponseDto> CreateTaskAsync(CreateTaskRequestDto request)
    {
        // Obtenemos la hora actual para establecer las fechas de creación y actualización
        var currentTime = _dateTimeProvider.UtcNow;
        // Creamos una nueva instancia de TaskItem utilizando los datos del DTO de solicitud y la hora actual
        var newTask = new TaskItem(request.Title, request.Description, request.Status, currentTime);
        // Agregamos la nueva tarea al repositorio y obtenemos la tarea creada con su ID generado
        var createdTask = await _taskRepo.AddAsync(newTask);
        // Mapeamos la tarea creada a un DTO de respuesta para ser consumido por el frontend y lo devolvemos
        return createdTask.Adapt<TaskResponseDto>();
    }

    /// <summary>
    /// Actualiza una tarea existente en la base de datos utilizando los datos proporcionados en el DTO de solicitud.
    /// Verifica si la tarea existe antes de intentar actualizarla y devuelve null si no se puede actualizar.
    /// Mapea la tarea actualizada a un DTO de respuesta para ser consumido por el frontend y lo devuelve.
    /// </summary>
    /// <param name="id">El ID de la tarea a actualizar.</param>
    /// <param name="request">El DTO de solicitud con los datos para actualizar la tarea.</param>
    /// <returns>El DTO de respuesta con los datos de la tarea actualizada o null si no se pudo actualizar.</returns>
    public async Task<TaskResponseDto?> UpdateTaskAsync(Guid id, UpdateTaskRequestDto request)
    {
        // Obtenemos la tarea existente por su ID para verificar si existe antes de intentar actualizarla
        var existingTask = await _taskRepo.GetByIdAsync(id);

        // Si la tarea no existe, devolvemos null para indicar que no se pudo actualizar
        if (existingTask == null)
            return null;

        // Mapeamos los datos del DTO de solicitud a la tarea existente
        // para actualizar sus propiedades con los nuevos valores proporcionados
        request.Adapt(existingTask);

        // Actualizamos la fecha de actualización de la tarea a la hora actual para reflejar el cambio
        existingTask.UpdatedAt = _dateTimeProvider.UtcNow;

        // Guardamos los cambios en el repositorio actualizando la tarea existente
        await _taskRepo.UpdateAsync(existingTask);

        // Mapeamos la tarea actualizada a un DTO de respuesta para ser consumido por el frontend y lo devolvemos
        return existingTask.Adapt<TaskResponseDto>();
    }

    /// <summary>
    /// Actualiza el estado de una tarea existente en la base de datos utilizando los datos proporcionados en el DTO de solicitud.
    /// Verifica si la tarea existe antes de intentar actualizar su estado y devuelve null si no se puede actualizar.
    /// Mapea la tarea actualizada a un DTO de respuesta para ser consumido por el frontend y lo devuelve.
    /// </summary>
    /// <param name="id">El ID de la tarea a actualizar.</param>
    /// <param name="request">El DTO de solicitud con los datos para actualizar el estado de la tarea.</param>
    /// <returns>El DTO de respuesta con los datos de la tarea actualizada o null si no se pudo actualizar.</returns>
    public async Task<TaskResponseDto?> UpdateTaskStatusAsync(
        Guid id,
        UpdateTaskStatusRequestDto request
    )
    {
        // Obtenemos la tarea existente por su ID para verificar si existe antes de intentar actualizar su estado
        var existingTask = await _taskRepo.GetByIdAsync(id);

        // Si la tarea no existe, devolvemos null para indicar que no se pudo actualizar
        if (existingTask == null)
            return null;

        // Actualizamos el estado de la tarea con el nuevo valor proporcionado en el DTO de solicitud
        existingTask.Status = request.Status;

        // Actualizamos la fecha de actualización de la tarea a la hora actual para reflejar el cambio
        existingTask.UpdatedAt = _dateTimeProvider.UtcNow;

        // Guardamos los cambios en el repositorio actualizando la tarea existente
        await _taskRepo.UpdateAsync(existingTask);

        // Mapeamos la tarea actualizada a un DTO de respuesta para ser consumido por el frontend y lo devolvemos
        return existingTask.Adapt<TaskResponseDto>();
    }

    /// <summary>
    /// Elimina una tarea existente en la base de datos por su ID. Verifica si la tarea existe antes de intentar eliminarla y devuelve false si no se puede eliminar.
    /// </summary>
    /// <param name="id">El ID de la tarea a eliminar.</param>
    /// <returns>True si la tarea fue eliminada, false en caso contrario.</returns>
    public async Task<bool> DeleteTaskAsync(Guid id)
    {
        return await _taskRepo.DeleteAsync(id);
    }
}
