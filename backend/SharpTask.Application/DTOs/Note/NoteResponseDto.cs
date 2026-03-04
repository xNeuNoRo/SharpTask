namespace SharpTask.Application.DTOs.Note;

public record NoteResponseDto
{
    public Guid Id { get; init; }
    public Guid TaskId { get; init; }
    public required string Content { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}
