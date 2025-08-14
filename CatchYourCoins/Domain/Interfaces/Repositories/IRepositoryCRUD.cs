using Domain.Dashboard.Entities;
using Domain.Dashboard.Specifications;

namespace Domain.Interfaces.Repositories;

public interface IRepositoryCRUD<TEntity>
    where TEntity : DashboardEntity
{
    Task CreateAsync(TEntity entity, CancellationToken cancellationToken);
    Task CreateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken);
    Task<List<TEntity>> GetAsync(ISpecificationDashboardEntity<TEntity> specification, CancellationToken cancellationToken);
    void Update(IEnumerable<TEntity> entities);
    void Delete(IEnumerable<TEntity> entities);
}