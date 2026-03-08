using FluentValidation;
using SharpTask.Application.DTOs.Task;
using SharpTask.Application.Validators.Base;

namespace SharpTask.Application.Validators.Task;

/// <summary>
/// Validador para la creación de una tarea, que hereda las reglas de validación definidas en TaskRequestBaseValidator,
/// asegurando que el título no esté vacío, que la descripción no exceda los 1024 caracteres
/// y que el estado de la tarea sea un valor válido del enum TaskState,
/// cumpliendo así con los requisitos establecidos para las solicitudes de creación de tareas.
/// </summary>
public class CreateTaskValidator : TaskRequestBaseValidator<CreateTaskRequestDto>
{
    /// <summary>
    /// Constructor que define las reglas de validación específicas para la creación de una tarea,
    /// asegurando que la fecha de vencimiento no sea una fecha pasada, lo que garantiza
    /// que las tareas creadas tengan una fecha de vencimiento válida y no se asignen fechas que ya hayan pasado.
    /// </summary>
    public CreateTaskValidator()
    {
        RuleFor(x => x.DueDate)
            .GreaterThanOrEqualTo(DateTime.Today)
            .WithMessage("La fecha límite no puede ser una fecha pasada");
    }
}
