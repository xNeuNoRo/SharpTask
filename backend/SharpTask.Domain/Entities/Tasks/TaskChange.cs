using SharpTask.Domain.Enums;

namespace SharpTask.Domain.Entities.Tasks;

public class TaskChange
{
    public TaskState Status { get; init; }
    public DateTime ChangedAt { get; init; }

    public TaskChange() { }

    public TaskChange(TaskState status, DateTime now)
    {
        Status = status;
        ChangedAt = now;
    }
}
