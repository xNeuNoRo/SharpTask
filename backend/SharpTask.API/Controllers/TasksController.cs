using Microsoft.AspNetCore.Mvc;
using SharpTask.API.Controllers.Base;
using SharpTask.Application.DTOs.Task;
using SharpTask.Application.Interfaces.Services;
using SharpTask.Domain.Common;
using SharpTask.Domain.Enums;
using SharpTask.Domain.Exceptions;

namespace SharpTask.API.Controllers;

/// <summary>
/// Controlador de API para gestionar operaciones CRUD de tareas.
/// Proporciona endpoints para crear, leer, actualizar y eliminar tareas,
/// así como para filtrar, buscar y cambiar el estado de las tareas.
/// </summary>
public class TasksController : BaseApiController
{
    private readonly ITaskQueryService _queryService;
    private readonly ITaskCommandService _commandService;

    /// <remarks>
    /// Constructor del controlador de tareas, recibe las dependencias de los
    /// servicios de consulta y escritura de tareas a través de inyección de dependencias.
    /// </remarks>
    /// <param name="queryService">Servicio de consulta de tareas</param>
    /// <param name="commandService">Servicio de escritura de tareas</param>
    public TasksController(ITaskQueryService queryService, ITaskCommandService commandService)
    {
        _queryService = queryService;
        _commandService = commandService;
    }

    /// <remarks>
    /// Acción para obtener todas las tareas, responde a una solicitud GET a la ruta "api/v1/tasks",
    /// esta acción también permite filtrar las tareas por su estado, mediante un parámetro de
    /// consulta opcional llamado "status", que representa el estado de las tareas a obtener, y debe ser un valor
    /// del enum TaskState, si no se proporciona el parámetro de consulta "status",
    /// esta acción devuelve todas las tareas sin filtrar por estado.
    /// </remarks>
    /// <param name="status">El estado de las tareas a obtener (opcional)</param>
    /// <returns>Una respuesta HTTP con el listado de tareas</returns>
    [HttpGet]
    [ProducesResponseType(
        typeof(ApiResponse<IEnumerable<TaskResponseDto>>),
        StatusCodes.Status200OK
    )]
    public async Task<IActionResult> GetAll([FromQuery] TaskState? status)
    {
        var tasks = status.HasValue
            ? await _queryService.GetTasksByStatusAsync(status.Value)
            : await _queryService.GetAllTasksAsync();
        return Success(tasks);
    }

    /// <remarks>
    /// Acción para obtener una tarea por su ID, responde a una solicitud GET a la ruta "api/v1/tasks/{id}",
    /// donde {id} es un parámetro de ruta que representa el ID de la tarea a obtener, y debe ser un GUID.
    /// </remarks>
    /// <param name="id">El ID de la tarea a obtener</param>
    /// <returns>Una respuesta HTTP con la tarea obtenida o un error 404 si no se encuentra</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<TaskDetailResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var task = await _queryService.GetTaskByIdAsync(id);
        return SuccessOrNotFound(task);
    }

    /// <remarks>
    /// Acción para crear una nueva tarea, responde a una solicitud POST a la ruta "api/v1/tasks",
    /// y recibe un objeto JSON en el cuerpo de la solicitud que representa los datos
    /// de la tarea a crear, con el formato definido por CreateTaskRequestDto.
    /// </remarks>
    /// <param name="request">Los datos de la tarea a crear</param>
    /// <returns>Una respuesta HTTP con la tarea creada</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<TaskResponseDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateTaskRequestDto request)
    {
        var task = await _commandService.CreateTaskAsync(request);
        return CreatedSuccess(nameof(GetById), new { id = task.Id }, task);
    }

    /// <remarks>
    /// Acción para actualizar una tarea existente, responde a una solicitud PUT a la ruta "api/v1/tasks/{id}",
    /// donde {id} es un parámetro de ruta que representa el ID de la tarea a actualizar, y debe ser un GUID.
    /// </remarks>
    /// <param name="id">El ID de la tarea a actualizar</param>
    /// <param name="request">Los datos actualizados de la tarea</param>
    /// <returns>Una respuesta HTTP con la tarea actualizada o un error 404 si no se encuentra</returns>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<TaskResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTaskRequestDto request)
    {
        var task = await _commandService.UpdateTaskAsync(id, request);
        return SuccessOrNotFound(
            task, // La task actualizada
            ErrorCodes.TaskNotFound, // El código de error específico para tarea no encontrada
            "No se encontró la tarea a actualizar." // El mensaje de error específico para tarea no encontrada
        );
    }

    /// <remarks>
    /// Acción para actualizar el estado de una tarea existente,
    /// responde a una solicitud PATCH a la ruta "api/v1/tasks/{id}/status",
    /// donde {id} es un parámetro de ruta que representa el ID de la tarea a actualizar,
    /// y debe ser un GUID, y recibe un objeto JSON en el cuerpo de la solicitud que representa
    /// el nuevo estado de la tarea, con el formato definido por UpdateTaskStatusRequestDto.
    /// </remarks>
    /// <param name="id">El ID de la tarea a actualizar</param>
    /// <param name="request">Los datos del nuevo estado de la tarea</param>
    /// <returns>Una respuesta HTTP con la tarea actualizada o un error 404 si no se encuentra</returns>
    [HttpPatch("{id:guid}/status")]
    [ProducesResponseType(typeof(ApiResponse<TaskResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateStatus(
        Guid id,
        [FromBody] UpdateTaskStatusRequestDto request
    )
    {
        var task = await _commandService.UpdateTaskStatusAsync(id, request);
        return SuccessOrNotFound(
            task, // La task actualizada
            ErrorCodes.TaskNotFound, // El código de error específico para tarea no encontrada
            "No se encontró la tarea a actualizar." // El mensaje de error específico para tarea no encontrada
        );
    }

    /// <remarks>
    /// Acción para marcar una tarea como completada,
    /// responde a una solicitud PATCH a la ruta "api/v1/tasks/{id}/complete",
    /// donde {id} es un parámetro de ruta que representa el ID de la tarea
    /// a actualizar, y debe ser un GUID, esta acción no recibe un cuerpo
    /// en la solicitud, ya que el nuevo estado de la tarea siempre será TaskState.Completed,
    /// definido en el método como un valor fijo, por lo que no es necesario recibirlo en la solicitud.
    /// </remarks>
    /// <param name="id">El ID de la tarea a actualizar</param>
    /// <returns>Una respuesta HTTP con la tarea actualizada o un error 404 si no se encuentra</returns>
    [HttpPatch("{id:guid}/complete")]
    [ProducesResponseType(typeof(ApiResponse<TaskResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Complete(Guid id)
    {
        var task = await _commandService.CompleteTaskAsync(id);
        return SuccessOrNotFound(
            task, // La task actualizada
            ErrorCodes.TaskNotFound, // El código de error específico para tarea no encontrada
            "No se encontró la tarea a actualizar." // El mensaje de error específico para tarea no encontrada
        );
    }

    /// <remarks>
    /// Acción para eliminar una tarea existente, responde a una solicitud DELETE a la ruta "api/v1/tasks/{id}",
    /// donde {id} es un parámetro de ruta que representa el ID de la tarea
    /// a eliminar, y debe ser un GUID, esta acción no recibe un cuerpo en la solicitud.
    /// </remarks>
    /// <param name="id">El ID de la tarea a eliminar</param>
    /// <returns>Una respuesta HTTP con el resultado de la operación de eliminación o un error 404 si no se encuentra</returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _commandService.DeleteTaskAsync(id);
        return SuccessOrNotFound(
            deleted, // El resultado de la operación de eliminación
            ErrorCodes.TaskNotFound, // El código de error específico para tarea no encontrada
            "No se pudo eliminar porque la tarea no existe." // El mensaje de error específico para tarea no encontrada
        );
    }

    /// <remarks>
    /// Acción para buscar tareas por una palabra clave, responde a una solicitud GET a la ruta "api/v1/tasks/search",
    /// y recibe un parámetro de consulta llamado "keyword" que representa la palabra clave para
    /// buscar en el título o descripción de las tareas, esta acción devuelve una lista de tareas que coinciden
    /// con la palabra clave, o una lista vacía si no se encuentran coincidencias.
    /// </remarks>
    /// <param name="keyword">La palabra clave para buscar en el título o descripción de las tareas</param>
    /// <returns>Una lista de tareas que coinciden con la palabra clave o una lista vacía si no se encuentran coincidencias</returns>
    [HttpGet("search")]
    [ProducesResponseType(
        typeof(ApiResponse<IEnumerable<TaskResponseDto>>),
        StatusCodes.Status200OK
    )]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SearchByKeyword([FromQuery] string keyword)
    {
        var tasks = await _queryService.SearchTasksAsync(keyword);
        return Success(tasks);
    }
}
