namespace Domain.Interfaces.Repositories;

public interface IRepositoryCRUD<T>
{
    Task CreateAsync(T entity);
    Task<T?> GetByIdAsync(int id);
    void Delete(T entity);
}