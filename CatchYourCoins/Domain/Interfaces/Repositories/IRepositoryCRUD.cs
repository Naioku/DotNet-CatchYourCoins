namespace Domain.Interfaces.Repositories;

public interface IRepositoryCRUD<T>
{
    Task CreateAsync(T entity);
    Task CreateRangeAsync(IEnumerable<T> entities);
    Task<T?> GetByIdAsync(int id);
    Task<List<T>> GetAllAsync();
    void Delete(T entity);
}