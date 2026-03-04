using SharpTask.Domain.Enums;

namespace SharpTask.Application.DTOs.Task;

public record TaskResponseDto
{
    public Guid Id { get; init; }
    public required string Title { get; init; }
    public string? Description { get; init; }
    public TaskState Status { get; init; }
    public List<TaskChangeDto> Changes { get; init; } = new();
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}
