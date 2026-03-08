using System.Diagnostics.CodeAnalysis;
using SharpTask.Domain.Entities.Base;

namespace SharpTask.Domain.Entities;

/// <summary>
/// Clase que representa una nota (NoteItem) asociada a una tarea (TaskItem) en el sistema,
/// con propiedades para el contenido de la nota (Content) y una referencia al Id de la
/// tarea a la que está asociada (TaskId), heredando de BaseEntity
/// para incluir propiedades comunes como Id, CreatedAt y UpdatedAt.
/// </summary>
public class NoteItem : BaseEntity
{
    /// <summary>
    /// Identificador de la tarea a la que está asociada la nota.
    /// </summary>
    /// <example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
    public Guid TaskId { get; init; }

    /// <summary>
    /// Contenido de la nota, es un campo requerido que contiene el texto de la nota asociada a la tarea.
    /// </summary>
    /// <example>Esta es una nota importante sobre la tarea.</example>
    public required string Content { get; set; }

    /// <summary>
    /// Constructor para crear un objeto NoteItem sin parámetros,
    /// necesario para la deserialización de datos.
    /// </summary>
    public NoteItem() { }

    /// <summary>
    /// Constructor para crear un objeto NoteItem con los datos necesarios,
    /// recibe el Id de la tarea asociada, el contenido de la nota y la fecha de creación,
    /// y asigna estos valores a las propiedades correspondientes,
    /// además de generar un nuevo Id para la nota y establecer las fechas de creación y actualización.
    /// </summary>
    /// <param name="taskId">El identificador de la tarea a la que está asociada la nota.</param>
    /// <param name="content">El contenido de la nota.</param>
    /// <param name="now">La fecha y hora de creación.</param>
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
