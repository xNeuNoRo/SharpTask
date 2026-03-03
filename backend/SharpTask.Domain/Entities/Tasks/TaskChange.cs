using SharpTask.Domain.Enums;

namespace SharpTask.Domain.Entities.Tasks;

public class TaskChange
{
    public TaskState Status { get; init; }
    public DateTime ChangedAt { get; init; }
}
