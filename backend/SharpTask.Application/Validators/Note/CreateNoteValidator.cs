using SharpTask.Application.DTOs.Note;
using SharpTask.Application.Validators.Base;

namespace SharpTask.Application.Validators.Note;

/// <summary>
/// Validador para la creación de una nota, que hereda las reglas de validación definidas en NoteRequestBaseValidator,
/// asegurando que el contenido de la nota cumpla con los requisitos establecidos para las solicitudes de notas.
/// </summary>
public class CreateNoteValidator : NoteRequestBaseValidator<CreateNoteRequestDto> { }
