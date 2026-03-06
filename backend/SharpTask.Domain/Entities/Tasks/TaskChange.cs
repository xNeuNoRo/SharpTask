using SharpTask.Domain.Enums;

namespace SharpTask.Domain.Entities.Tasks;

/// <summary>
/// Clase que representa un cambio de estado en una tarea,
/// con propiedades para el nuevo estado (Status) y la fecha y hora del cambio (ChangedAt).
/// </summary>
public class TaskChange
{
    /// <summary>
    /// Nuevo estado de la tarea después del cambio, representado por el enum TaskState.
    /// </summary>
    /// <example>InProgress</example>
    public TaskState Status { get; init; }

    /// <summary>
    /// Fecha y hora del cambio de estado.
    /// </summary>
    /// <example>2026-06-01T12:00:00Z</example>
    public DateTime ChangedAt { get; init; }

    /// <summary>
    /// Constructor para crear un objeto TaskChange sin parámetros,
    /// necesario para la deserialización de datos.
    /// </summary>
    public TaskChange() { }

    /// <summary>
    /// Constructor para crear un objeto TaskChange con el nuevo estado y la fecha del cambio,
    /// recibe un TaskState y un DateTime, y los asigna a las propiedades correspondientes.
    /// </summary>
    /// <param name="status">El nuevo estado de la tarea.</param>
    /// <param name="now">La fecha y hora del cambio de estado.</param>
    public TaskChange(TaskState status, DateTime now)
    {
        Status = status;
        ChangedAt = now;
    }
}
