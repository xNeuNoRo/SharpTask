using FluentValidation;
using SharpTask.Application.DTOs.Base;

namespace SharpTask.Application.Validators.Base;

/// <remarks>
/// Clase base para validadores de solicitudes relacionadas con notas, utilizando FluentValidation.
/// </remarks>
/// <typeparam name="T">El tipo de solicitud base para notas.</typeparam>
public class NoteRequestBaseValidator<T> : AbstractValidator<T>
    where T : NoteRequestBase
{
    public NoteRequestBaseValidator()
    {
        RuleFor(x => x.Content)
            .NotEmpty()
            .WithMessage("El contenido de la nota es obligatorio.")
            .MaximumLength(2048)
            .WithMessage("El contenido de la nota no puede exceder los 2048 caracteres.");
    }
}
