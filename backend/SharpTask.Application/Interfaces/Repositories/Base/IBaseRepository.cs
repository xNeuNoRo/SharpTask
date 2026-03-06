namespace SharpTask.Application.Interfaces.Repositories.Base;

/// <remarks>
/// Interfaz generica para el repositorio base que define las operaciones CRUD comunes para cualquier entidad.
/// </remarks>
/// <typeparam name="T">Tipo de entidad que implementa esta interfaz</typeparam>
public interface IBaseRepository<T>
    where T : class
{
    // Contrato para las operaciones CRUD basicas que cualquier repositorio debe implementar

    /// <summary>
    /// Obtiene todas las entidades del tipo T
    /// </summary>
    /// <returns>Una lista de entidades del tipo T</returns>
    Task<IEnumerable<T>> GetAllAsync();

    /// <summary>
    /// Obtiene una entidad del tipo T por su identificador único (GUID)
    /// </summary>
    /// <param name="id">El identificador único de la entidad</param>
    /// <returns>La entidad encontrada o null si no se encuentra</returns>
    Task<T?> GetByIdAsync(Guid id);

    /// <summary>
    /// Agrega una nueva entidad del tipo T a la base de datos
    /// </summary>
    /// <param name="entity">La entidad a agregar</param>
    /// <returns>La entidad agregada</returns>
    Task<T> AddAsync(T entity);

    /// <summary>
    /// Actualiza una entidad existente del tipo T en la base de datos
    /// </summary>
    /// <param name="entity">La entidad a actualizar</param>
    /// <returns>La entidad actualizada o null si no se encuentra</returns>
    Task<T?> UpdateAsync(T entity);

    /// <summary>
    /// Elimina una entidad del tipo T de la base de datos por su identificador único (GUID)
    /// </summary>
    /// <param name="id">El identificador único de la entidad</param>
    /// <returns>True si la entidad fue eliminada, false en caso contrario</returns>
    Task<bool> DeleteAsync(Guid id);

    /// <summary>
    /// Verifica si una entidad del tipo T existe en la base de datos por su identificador único (GUID)
    /// </summary>
    /// <param name="id">El identificador único de la entidad</param>
    /// <returns>True si la entidad existe, false en caso contrario</returns>
    Task<bool> ExistsAsync(Guid id);
}
