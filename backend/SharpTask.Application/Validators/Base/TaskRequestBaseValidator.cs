using FluentValidation;
using SharpTask.Application.DTOs.Base;

namespace SharpTask.Application.Validators.Base;

/// <remarks>
/// Clase base para validadores de solicitudes relacionadas con tareas, utilizando FluentValidation.
/// </remarks>
/// <typeparam name="T">El tipo de solicitud a validar.</typeparam>
public class TaskRequestBaseValidator<T> : AbstractValidator<T>
    where T : TaskRequestBase
{
    /// <summary>
    /// Constructor que define las reglas de validación para las solicitudes de tareas,
    /// asegurando que el título no esté vacío, que la descripción no exceda los
    /// 1024 caracteres y que el estado de la tarea sea un valor válido del enum TaskState.
    /// </summary>
    public TaskRequestBaseValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("El título es obligatorio.")
            .MaximumLength(256)
            .WithMessage("El título no puede exceder los 256 caracteres.");

        RuleFor(x => x.Description)
            .MaximumLength(1024)
            .WithMessage("La descripción no puede exceder los 1024 caracteres.");

        RuleFor(x => x.Status)
            .IsInEnum()
            .WithMessage("El estado de la tarea no es válido.")
            .When(x => x.Status.HasValue);
    }
}
