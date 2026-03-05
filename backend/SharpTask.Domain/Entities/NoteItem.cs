using System.Diagnostics.CodeAnalysis;
using SharpTask.Domain.Entities.Base;

namespace SharpTask.Domain.Entities;

public class NoteItem : BaseEntity
{
    public Guid TaskId { get; init; }
    public required string Content { get; set; }

    public NoteItem() { }

    [SetsRequiredMembers]
    public NoteItem(Guid taskId, string content, DateTime now)
    {
        Id = Guid.NewGuid();
        TaskId = taskId;
        Content = content;
        CreatedAt = now;
        UpdatedAt = now;
    }
}
