using SharpTask.Domain.Enums;

namespace SharpTask.Application.DTOs.Base;

/// <summary>
/// Clase base para las solicitudes de tareas
/// </summary>
public abstract record TaskRequestBase
{
    /// <summary>
    /// Título de la tarea
    /// </summary>
    /// <example>Comprar leche</example>
    public required string Title { get; init; }

    /// <summary>
    /// Descripción de la tarea
    /// </summary>
    /// <example>Comprar leche en el supermercado</example>
    public string? Description { get; init; }

    /// <summary>
    /// Estado de la tarea
    /// </summary>
    /// <example>Pending</example>
    public TaskState? Status { get; init; }
}
