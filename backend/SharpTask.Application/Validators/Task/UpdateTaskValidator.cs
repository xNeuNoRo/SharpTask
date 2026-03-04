using SharpTask.Application.DTOs.Task;
using SharpTask.Application.Validators.Base;

namespace SharpTask.Application.Validators.Task;

public class UpdateTaskValidator : TaskRequestBaseValidator<UpdateTaskRequestDto>
{
    public UpdateTaskValidator() { }
}
