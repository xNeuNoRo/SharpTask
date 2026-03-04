using SharpTask.Application.DTOs.Note;
using SharpTask.Application.Validators.Base;

namespace SharpTask.Application.Validators.Note;

public class UpdateNoteValidator : NoteRequestBaseValidator<UpdateNoteRequestDto>
{
    public UpdateNoteValidator() { }
}
