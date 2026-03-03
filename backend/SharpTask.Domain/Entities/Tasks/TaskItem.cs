using SharpTask.Domain.Entities.Base;
using SharpTask.Domain.Enums;

namespace SharpTask.Domain.Entities.Tasks;

public class TaskItem : BaseEntity
{
    public required string Title { get; set; }
    public string? Description { get; set; }
    public TaskState Status { get; set; }
    public List<TaskChange> Changes { get; set; } = new();
}
