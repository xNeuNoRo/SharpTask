using System.Diagnostics.CodeAnalysis;
using SharpTask.Domain.Entities.Base;
using SharpTask.Domain.Enums;

namespace SharpTask.Domain.Entities.Tasks;

/// <summary>
/// Clase que representa una tarea (TaskItem) en el sistema,
/// con propiedades para el título, descripción, estado actual
/// y un historial de cambios de estado (TaskChange),
/// heredando de BaseEntity para incluir propiedades comunes como Id, CreatedAt y UpdatedAt.
/// </summary>
public class TaskItem : BaseEntity
{
    /// <summary>
    /// Título de la tarea, es un campo requerido que describe brevemente la tarea a realizar.
    /// </summary>
    /// <example>Comprar víveres</example>
    public required string Title { get; set; }

    /// <summary>
    /// Descripción de la tarea, es un campo opcional que proporciona detalles adicionales sobre la tarea a realizar.
    /// </summary>
    /// <example>Comprar leche, pan y huevos para la semana.</example>
    public string? Description { get; set; }

    /// <summary>
    /// Fecha de vencimiento de la tarea, es un campo opcional
    /// que indica la fecha límite para completar la tarea,
    /// </summary>
    /// <example>2026-06-30T23:59:00Z</example>
    public DateTime? DueDate { get; set; }

    /// <summary>
    /// Estado actual de la tarea, representado por el enum TaskState,
    /// que puede ser Pending, InProgress, OnHold, UnderReview o Completed.
    /// </summary>
    /// <example>Pending</example>
    public TaskState Status { get; set; }

    /// <summary>
    /// Historial de cambios de estado de la tarea,
    /// representado por una lista de objetos TaskChange,
    /// que registra cada cambio de estado con su nuevo estado
    /// y la fecha del cambio, lo que permite llevar un seguimiento completo de la evolución de la tarea a lo largo del tiempo.
    /// </summary>
    /// <example> [{"Status": "InProgress", "ChangedAt": "2026-06-01T12:00:00Z"}, {"Status": "Completed", "ChangedAt": "2026-06-02T15:30:00Z"}] </example>
    public List<TaskChange> Changes { get; set; } = new();

    /// <summary>
    /// Constructor para crear un objeto TaskItem sin parámetros,
    /// necesario para la deserialización de datos.
    /// </summary>
    public TaskItem() { }

    /// <summary>
    /// Constructor para crear un objeto TaskItem con los datos necesarios,
    /// recibe el título, descripción, estado inicial y la fecha de creación,
    /// y asigna estos valores a las propiedades correspondientes,
    /// además de generar un nuevo Id y agregar un cambio inicial al historial de cambios.
    /// </summary>
    /// <param name="title">El título de la tarea.</param>
    /// <param name="description">La descripción de la tarea.</param>
    /// <param name="dueDate">La fecha de vencimiento de la tarea.</param>
    /// <param name="status">El estado inicial de la tarea.</param>
    /// <param name="now">La fecha y hora de creación.</param>
    [SetsRequiredMembers]
    public TaskItem(
        string title,
        string? description,
        DateTime? dueDate,
        TaskState? status,
        DateTime now
    )
    {
        Id = Guid.NewGuid();
        Title = title;
        Description = description;
        DueDate = dueDate;
        Status = status ?? TaskState.Pending;
        CreatedAt = now;
        UpdatedAt = now;

        // Agregamos un cambio inicial para reflejar la creación de la tarea
        Changes.Add(new TaskChange(Status, now));
    }
}
