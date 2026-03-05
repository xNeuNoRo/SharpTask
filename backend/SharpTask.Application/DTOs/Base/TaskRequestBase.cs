using SharpTask.Domain.Enums;

namespace SharpTask.Application.DTOs.Base;

public abstract record TaskRequestBase
{
    public required string Title { get; init; }
    public string? Description { get; init; }
    public TaskState? Status { get; init; }
}
