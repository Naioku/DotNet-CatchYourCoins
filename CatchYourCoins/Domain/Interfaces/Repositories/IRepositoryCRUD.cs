using Domain.Dashboard.Entities;
using Domain.Dashboard.Specifications;

namespace Domain.Interfaces.Repositories;

public interface IRepositoryCRUD<TEntity>
    where TEntity : DashboardEntity
{
    Task CreateAsync(TEntity entity);
    Task CreateRangeAsync(IEnumerable<TEntity> entities);
    Task<List<TEntity>> GetAsync(ISpecificationDashboardEntity<TEntity> specification);
    void Update(IEnumerable<TEntity> entities);
    void Delete(IEnumerable<TEntity> entities);
}