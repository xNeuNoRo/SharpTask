using SharpTask.Domain.Entities.Base;

namespace SharpTask.Domain.Entities.Tasks;

public class TaskItem : BaseEntity
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public TaskStatus Status { get; set; }
    public List<TaskChange> Changes { get; set; } = new();
}
