namespace SharpTask.Domain.Entities.Tasks;

public class TaskChange
{
    public TaskStatus Status { get; init; }
    public DateTime ChangedAt { get; init; }
}
