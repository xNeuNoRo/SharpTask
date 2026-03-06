using FluentValidation;
using SharpTask.Application.DTOs.Task;

namespace SharpTask.Application.Validators.Task;

/// <summary>
/// Validador para la actualización del estado de una tarea,
/// que asegura que el nuevo estado proporcionado sea un valor válido del enum TaskState,
/// cumpliendo así con los requisitos establecidos para las solicitudes de actualización del estado de las tareas.
/// </summary>
public class UpdateTaskStatusValidator : AbstractValidator<UpdateTaskStatusRequestDto>
{
    /// <summary>
    /// Constructor que define las reglas de validación para la actualización del estado de una tarea,
    /// asegurando que el nuevo estado proporcionado sea un valor válido del enum TaskState,
    /// cumpliendo así con los requisitos establecidos para las solicitudes de actualización del estado de las tareas.
    /// </summary>
    public UpdateTaskStatusValidator()
    {
        RuleFor(x => x.Status).IsInEnum().WithMessage("El estado de la tarea no es válido.");
    }
}
