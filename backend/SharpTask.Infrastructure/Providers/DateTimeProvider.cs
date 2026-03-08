using SharpTask.Domain.Interfaces;

namespace SharpTask.Infrastructure.Providers;

/// <summary>
/// Implementación concreta de la interfaz IDateTimeProvider
/// </summary>
public class DateTimeProvider : IDateTimeProvider
{
    /// <summary>
    /// Propiedad que devuelve la fecha y hora actual en formato UTC
    /// </summary>
    /// <example>2026-06-01T12:00:00Z</example>
    public DateTime UtcNow => DateTime.UtcNow;
}
