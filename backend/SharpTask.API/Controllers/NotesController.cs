using Microsoft.AspNetCore.Mvc;
using SharpTask.API.Controllers.Base;
using SharpTask.Application.DTOs.Note;
using SharpTask.Application.Interfaces.Services;
using SharpTask.Domain.Common;
using SharpTask.Domain.Exceptions;

namespace SharpTask.API.Controllers;

// Este controlador por logica debe vivir bajo esta ruta,
// Ya que estamos hablando de un subrecurso de tareas,
// que son las notas asociadas a una tarea específica,
// por lo que la ruta debe reflejar esa relación jerárquica entre tareas y notas.
// Manteniendo los principios de una RESTful API

/// <summary>
/// Controlador de API para gestionar operaciones CRUD de notas asociadas a tareas específicas.
/// Proporciona endpoints para crear, leer, actualizar y eliminar notas,
/// siempre y cuando esas notas estén asociadas a una tarea específica identificada por su ID en la ruta.
/// </summary>
[Route("api/v1/tasks/{taskId:guid}/[controller]")]
public class NotesController : BaseApiController
{
    private readonly INoteQueryService _queryService;
    private readonly INoteCommandService _commandService;

    /// <remarks>
    /// Constructor del controlador de notas, recibe las dependencias de los
    /// servicios de consulta y escritura de notas a través de inyección de dependencias.
    /// </remarks>
    /// <param name="queryService">Servicio de consulta de notas</param>
    /// <param name="commandService">Servicio de escritura de notas</param>
    public NotesController(INoteQueryService queryService, INoteCommandService commandService)
    {
        _queryService = queryService;
        _commandService = commandService;
    }

    /// <remarks>
    /// Acción para obtener todas las notas asociadas a una tarea específica,
    /// responde a una solicitud GET a la ruta "api/v1/tasks/{taskId}/notes",
    /// donde {taskId} es un parámetro de ruta que representa el ID de la
    /// tarea a la que pertenecen las notas a obtener, y debe ser un GUID,
    /// esta acción devuelve una lista de notas asociadas a la tarea especificada
    /// por el ID en la ruta.
    /// </remarks>
    /// <param name="taskId">ID de la tarea a la que pertenecen las notas</param>
    /// <returns>Una lista de notas asociadas a la tarea especificada</returns>
    [HttpGet]
    [ProducesResponseType(
        typeof(ApiResponse<IEnumerable<NoteResponseDto>>),
        StatusCodes.Status200OK
    )]
    public async Task<IActionResult> GetAll(Guid taskId)
    {
        var notes = await _queryService.GetNotesByTaskIdAsync(taskId);
        return Success(notes);
    }

    /// <remarks>
    /// Acción para obtener una nota por su ID, responde a una solicitud GET
    /// a la ruta "api/v1/tasks/{taskId}/notes/{id}", donde {taskId} es un
    /// parámetro de ruta que representa el ID de la tarea a la que pertenece la nota,
    /// y {id} es un parámetro de ruta que representa el ID de la nota a obtener,
    /// ambos deben ser GUIDs, esta acción devuelve la nota especificada por el ID en la ruta,
    /// siempre y cuando esa nota pertenezca a la tarea especificada por el taskId en la ruta,
    /// de lo contrario devuelve un error 404 indicando que la nota no se encontró.
    /// </remarks>
    /// <param name="taskId">ID de la tarea a la que pertenece la nota</param>
    /// <param name="id">ID de la nota a obtener</param>
    /// <returns>La nota especificada por el ID en la ruta</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<NoteResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid taskId, Guid id)
    {
        var note = await _queryService.GetNoteByIdAsync(id);
        return SuccessOrNotFound(note, ErrorCodes.NoteNotFound, "La nota solicitada no existe.");
    }

    /// <remarks>
    /// Acción para crear una nueva nota asociada a una tarea específica,
    /// responde a una solicitud POST a la ruta "api/v1/tasks/{taskId}/notes",
    /// donde {taskId} es un parámetro de ruta que representa el ID de la tarea a la que se asociará la nota,
    /// y debe ser un GUID, esta acción recibe un objeto JSON en el cuerpo de la solicitud que representa
    /// los datos de la nota a crear, con el formato definido por CreateNoteRequestDto y crea una nueva
    /// nota asociada a la tarea especificada por el taskId en la ruta, si la tarea especificada
    /// por el taskId no existe, devuelve un error 404 indicando que la tarea no se encontró.
    /// </remarks>
    /// <param name="taskId">ID de la tarea a la que se asociará la nota</param>
    /// <param name="request">Datos de la nota a crear</param>
    /// <returns>La nota creada</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<NoteResponseDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(Guid taskId, [FromBody] CreateNoteRequestDto request)
    {
        var note = await _commandService.CreateNoteAsync(taskId, request);
        return CreatedSuccess(nameof(GetById), new { taskId, id = note.Id }, note);
    }

    /// <remarks>
    /// Acción para actualizar una nota existente,
    /// responde a una solicitud PUT a la ruta "api/v1/tasks/{taskId}/notes/{id}",
    /// donde {taskId} es un parámetro de ruta que representa el ID de la tarea a
    /// la que pertenece la nota, y {id} es un parámetro de ruta que representa
    /// el ID de la nota a actualizar, ambos deben ser GUIDs, esta acción recibe
    /// un objeto JSON en el cuerpo de la solicitud que representa los datos actualizados de la nota,
    /// con el formato definido por UpdateNoteRequestDto y actualiza la nota especificada por el ID
    /// en la ruta, siempre y cuando esa nota pertenezca a la tarea especificada por el taskId en la ruta,
    /// de lo contrario devuelve un error 404 indicando que la nota no se encontró, esta acción también
    /// devuelve un error 404 si la tarea especificada por el taskId en la ruta no existe, indicando que la tarea no se encontró.
    /// </remarks>
    /// <param name="taskId">ID de la tarea a la que pertenece la nota</param>
    /// <param name="id">ID de la nota a actualizar</param>
    /// <param name="request">Datos actualizados de la nota</param>
    /// <returns>La nota actualizada</returns>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<NoteResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(
        Guid taskId,
        Guid id,
        [FromBody] UpdateNoteRequestDto request
    )
    {
        var note = await _commandService.UpdateNoteAsync(id, request);
        return SuccessOrNotFound(
            note,
            ErrorCodes.NoteNotFound,
            "No se encontró la nota a actualizar."
        );
    }

    /// <remarks>
    /// Acción para eliminar una nota existente,
    /// responde a una solicitud DELETE a la ruta "api/v1/tasks/{taskId}/notes/{id}",
    /// donde {taskId} es un parámetro de ruta que representa el ID de la tarea a la que
    /// pertenece la nota, y {id} es un parámetro de ruta que representa el ID de la nota a
    /// eliminar, ambos deben ser GUIDs, esta acción elimina la nota especificada por el ID
    /// en la ruta, siempre y cuando esa nota pertenezca a la tarea especificada por el taskId
    /// en la ruta, de lo contrario devuelve un error 404 indicando que la nota no se encontró,
    /// esta acción también devuelve un error 404 si la tarea especificada por el taskId en la ruta no existe,
    /// indicando que la tarea no se encontró.
    /// </remarks>
    /// <param name="taskId">ID de la tarea a la que pertenece la nota</param>
    /// <param name="id">ID de la nota a eliminar</param>
    /// <returns>Una respuesta HTTP indicando el resultado de la operación</returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid taskId, Guid id)
    {
        var deleted = await _commandService.DeleteNoteAsync(id);
        return SuccessOrNotFound(
            deleted,
            ErrorCodes.NoteNotFound,
            "No se pudo eliminar porque la nota no existe."
        );
    }
}
