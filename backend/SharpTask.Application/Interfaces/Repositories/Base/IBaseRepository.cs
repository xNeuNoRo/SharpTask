namespace SharpTask.Application.Interfaces.Repositories.Base;

/// <summary>
/// Interfaz generica para el repositorio base que define las operaciones CRUD comunes para cualquier entidad.
/// </summary>
/// <typeparam name="T">Tipo de entidad que implementa esta interfaz</typeparam>
public interface IBaseRepository<T>
    where T : class
{
    // Contrato para las operaciones CRUD basicas que cualquier repositorio debe implementar
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(Guid id);
    Task<T> AddAsync(T entity);
    Task<T?> UpdateAsync(T entity);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
}
