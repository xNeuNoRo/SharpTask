namespace SharpTask.Domain.Entities.Base;

/// <summary>
/// Clase base para todas las entidades del dominio,
/// que incluye propiedades comunes como Id, CreatedAt y UpdatedAt.
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// Identificador único de la entidad
    /// </summary>
    /// <example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
    public Guid Id { get; init; }

    /// <summary>
    /// Fecha y hora de creación de la entidad
    /// </summary>
    /// <example>2026-06-01T12:00:00Z</example>
    public DateTime CreatedAt { get; init; }

    /// <summary>
    /// Fecha y hora de la última actualización de la entidad
    /// </summary>
    /// <example>2026-06-01T12:00:00Z</example>
    public DateTime UpdatedAt { get; set; }
}
