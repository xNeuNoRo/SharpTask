using FluentValidation;
using SharpTask.Application.DTOs.Task;

namespace SharpTask.Application.Validators.Task;

public class UpdateTaskStatusValidator : AbstractValidator<UpdateTaskStatusRequestDto>
{
    public UpdateTaskStatusValidator()
    {
        RuleFor(x => x.Status).IsInEnum().WithMessage("El estado de la tarea no es válido.");
    }
}
