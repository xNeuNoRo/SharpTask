using SharpTask.Domain.Enums;

namespace SharpTask.Application.DTOs.Task;

public abstract record TaskChangeDto
{
    public TaskState Status { get; init; }
    public DateTime ChagedAt { get; init; }
}
