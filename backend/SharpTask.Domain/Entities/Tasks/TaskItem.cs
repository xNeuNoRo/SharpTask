using System.Diagnostics.CodeAnalysis;
using SharpTask.Domain.Entities.Base;
using SharpTask.Domain.Enums;

namespace SharpTask.Domain.Entities.Tasks;

public class TaskItem : BaseEntity
{
    public required string Title { get; set; }
    public string? Description { get; set; }
    public TaskState Status { get; set; }
    public List<TaskChange> Changes { get; set; } = new();

    public TaskItem() { }

    [SetsRequiredMembers]
    public TaskItem(string title, string? description, TaskState? status, DateTime now)
    {
        Id = Guid.NewGuid();
        Title = title;
        Description = description;
        Status = status ?? TaskState.Pending;
        CreatedAt = now;
        UpdatedAt = now;

        // Agregamos un cambio inicial para reflejar la creación de la tarea
        Changes.Add(new TaskChange(Status, now));
    }
}
