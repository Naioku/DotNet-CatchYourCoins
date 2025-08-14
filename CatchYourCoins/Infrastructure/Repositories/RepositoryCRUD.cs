using Domain.Dashboard.Entities;
using Domain.Dashboard.Specifications;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Infrastructure.Extensions;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class RepositoryCRUD<TEntity>(
    AppDbContext dbContext,
    IServiceCurrentUser serviceCurrentUser) : IRepositoryCRUD<TEntity>
    where TEntity : DashboardEntity
{
    public async Task CreateAsync(TEntity entity, CancellationToken cancellationToken) =>
        await dbContext.Set<TEntity>().AddAsync(entity, cancellationToken);

    public async Task CreateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken) =>
        await dbContext.Set<TEntity>().AddRangeAsync(entities, cancellationToken);

    public async Task<List<TEntity>> GetAsync(ISpecificationDashboardEntity<TEntity> specification, CancellationToken cancellationToken)
    {
        IQueryable<TEntity> query = dbContext.Set<TEntity>()
            .WhereAuthorized(serviceCurrentUser.User.Id)
            .Where(specification.Criteria);

        query = specification.Includes.Aggregate(
            query,
            (current, include) => current.Include(include)
        );

        query = specification.IncludeStrings.Aggregate(
            query,
            (current, include) => current.Include(include)
        );

        if (specification.OrderBy != null)
        {
            query = query.OrderBy(specification.OrderBy);
        }
        else if (specification.OrderByDescending != null)
        {
            query = query.OrderByDescending(specification.OrderByDescending);
        }

        if (specification.IsPagingEnabled)
        {
            query = query
                .Skip(specification.Skip)
                .Take(specification.Take);
        }

        return await query.ToListAsync(cancellationToken: cancellationToken);
    }

    public void Update(IEnumerable<TEntity> entities) => dbContext.Set<TEntity>().UpdateRange(entities);

    public void Delete(IEnumerable<TEntity> entities) => dbContext.Set<TEntity>().RemoveRange(entities);
}