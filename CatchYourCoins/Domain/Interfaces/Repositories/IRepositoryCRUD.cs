namespace Domain.Interfaces.Repositories;

public interface IRepositoryCRUD<TEntity>
{
    Task CreateAsync(TEntity entity);
    Task CreateRangeAsync(IEnumerable<TEntity> entities);
    Task<TEntity?> GetByIdAsync(int id);
    Task<List<TEntity>> GetAllAsync();
    void Update(TEntity entity);
    void Delete(TEntity entity);
}