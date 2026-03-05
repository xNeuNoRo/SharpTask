using SharpTask.Domain.Enums;

namespace SharpTask.Application.DTOs.Task;

public record UpdateTaskStatusRequestDto
{
    public required TaskState Status { get; init; }
}
