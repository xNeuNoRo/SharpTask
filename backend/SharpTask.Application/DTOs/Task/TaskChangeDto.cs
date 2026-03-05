using SharpTask.Domain.Enums;

namespace SharpTask.Application.DTOs.Task;

public record TaskChangeDto
{
    public TaskState Status { get; init; }
    public DateTime ChangedAt { get; init; }
}
