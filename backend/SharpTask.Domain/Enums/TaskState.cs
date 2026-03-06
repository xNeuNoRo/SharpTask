namespace SharpTask.Domain.Enums;

/// <summary>
/// Enumeración que representa los posibles estados de una tarea (TaskItem) en el sistema,
/// incluyendo estados como Pending (Pendiente),
/// OnHold (En espera), InProgress (En progreso),
/// UnderReview (En revisión) y Completed (Completada),
/// lo que permite categorizar y gestionar el flujo de trabajo de las tareas de manera eficiente.
/// </summary>
public enum TaskState
{
    /// <summary>
    /// Estado inicial de una tarea,
    /// que indica que la tarea está pendiente de ser iniciada.
    /// </summary>
    Pending,

    /// <summary>
    /// Estado que indica que la tarea está en espera, lo que significa
    /// que no se está trabajando activamente en ella,
    /// pero tampoco se ha completado,
    /// lo que puede ser útil para tareas que requieren una pausa.
    /// </summary>
    OnHold,

    /// <summary>
    /// Estado que indica que la tarea está en progreso,
    /// lo que significa que se está trabajando activamente en ella,
    /// pero aún no se ha completado, lo que permite identificar
    /// las tareas que están siendo ejecutadas en un momento dado.
    /// </summary>
    InProgress,

    /// <summary>
    /// Estado que indica que la tarea está en revisión, lo que significa
    /// que se ha completado el trabajo principal de la tarea,
    /// pero aún está siendo revisada o evaluada antes de ser marcada como completada,
    /// lo que puede ser útil para tareas que requieren una verificación adicional antes de considerarse finalizadas.
    /// </summary>
    UnderReview,

    /// <summary>
    /// Estado que indica que la tarea ha sido completada, lo que significa
    /// que se ha finalizado el trabajo relacionado con la tarea y se ha alcanzado
    /// el objetivo de la tarea, lo que permite identificar claramente las tareas que han sido finalizadas con éxito.
    /// </summary>
    Completed,
}
