using SharpTask.Application.DTOs.Task;
using SharpTask.Application.Validators.Base;

namespace SharpTask.Application.Validators.Task;

/// <summary>
/// Validador para la creación de una tarea, que hereda las reglas de validación definidas en TaskRequestBaseValidator,
/// asegurando que el título no esté vacío, que la descripción no exceda los 1024 caracteres 
/// y que el estado de la tarea sea un valor válido del enum TaskState, 
/// cumpliendo así con los requisitos establecidos para las solicitudes de creación de tareas.
/// </summary>
public class CreateTaskValidator : TaskRequestBaseValidator<CreateTaskRequestDto> { }
