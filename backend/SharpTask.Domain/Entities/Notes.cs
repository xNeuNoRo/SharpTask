using SharpTask.Domain.Entities.Base;

namespace SharpTask.Domain.Entities;

public class Notes : BaseEntity
{
    public Guid TaskId { get; init; }
    public required string Content { get; set; }
}
