using FluentValidation;
using SharpTask.Application.DTOs.Task;
using SharpTask.Application.Validators.Base;

namespace SharpTask.Application.Validators.Task;

/// <summary>
/// Validador para la actualización de una tarea, que hereda
/// las reglas de validación definidas en TaskRequestBaseValidator,
/// asegurando que el título no esté vacío, que la descripción no exceda
/// los 1024 caracteres y que el estado de la tarea sea un valor válido del enum TaskState.
/// </summary>
public class UpdateTaskValidator : TaskRequestBaseValidator<UpdateTaskRequestDto>
{
    /// <summary>
    /// Constructor que define las reglas de validación para la actualización de una tarea, 
    /// asegurando que el título no esté vacío, que la descripción no exceda 
    /// los 1024 caracteres y que el estado de la tarea sea un valor válido del enum TaskState.
    /// </summary>
    public UpdateTaskValidator()
    {
        RuleFor(x => x.Status)
            .NotNull()
            .WithMessage("El estado de la tarea es obligatorio al actualizar.");
    }
}
