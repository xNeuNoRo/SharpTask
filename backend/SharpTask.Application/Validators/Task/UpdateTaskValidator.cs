using FluentValidation;
using SharpTask.Application.DTOs.Task;
using SharpTask.Application.Validators.Base;

namespace SharpTask.Application.Validators.Task;

public class UpdateTaskValidator : TaskRequestBaseValidator<UpdateTaskRequestDto>
{
    public UpdateTaskValidator()
    {
        RuleFor(x => x.Status)
            .NotNull()
            .WithMessage("El estado de la tarea es obligatorio al actualizar.");
    }
}
