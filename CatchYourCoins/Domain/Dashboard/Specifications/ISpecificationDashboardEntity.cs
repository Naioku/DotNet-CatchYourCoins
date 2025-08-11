using System.Linq.Expressions;

namespace Domain.Dashboard.Specifications;

public interface ISpecificationDashboardEntity<TEntity>
    where TEntity : IEntity
{
    public Expression<Func<TEntity, bool>> Criteria { get; }
    public Expression<Func<TEntity, object>>? OrderBy { get; }
    public Expression<Func<TEntity, object>>? OrderByDescending { get; }
    public int Take { get; }
    public int Skip { get; }
    public bool IsPagingEnabled { get; }
    public IReadOnlyList<Expression<Func<TEntity, object>>> Includes { get; }
    public IReadOnlyList<string> IncludeStrings { get; }
}