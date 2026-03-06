namespace SharpTask.Application.DTOs.Base;

/// <summary>
/// Clase base para las solicitudes de notas
/// </summary>
public abstract record NoteRequestBase
{
    /// <summary>
    /// Contenido de la nota
    /// </summary>
    /// <example>Esta es una nota de ejemplo.</example>
    public required string Content { get; init; }
}
