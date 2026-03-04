namespace SharpTask.Application.DTOs.Base;

public abstract record NoteRequestBase
{
    public required string Content { get; init; }
}
