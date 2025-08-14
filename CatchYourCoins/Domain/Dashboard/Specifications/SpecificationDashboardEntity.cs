using System.Linq.Expressions;
using Domain.Dashboard.Entities;
using LinqKit;

namespace Domain.Dashboard.Specifications;

public abstract class SpecificationDashboardEntity<TEntity, TBuilder> : ISpecificationDashboardEntity<TEntity>
    where TEntity : DashboardEntity
    where TBuilder : SpecificationDashboardEntity<TEntity,TBuilder>.BuilderDashboardEntity
{
    private readonly List<Expression<Func<TEntity, object>>> _includes = [];
    private readonly List<string> _includeStrings = [];
    public Expression<Func<TEntity, bool>> Criteria { get; private set; }
    public Expression<Func<TEntity, object>>? OrderBy { get; private set; }
    public Expression<Func<TEntity, object>>? OrderByDescending { get; private set; }
    public int Take { get; private set; }
    public int Skip { get; private set; }
    public bool IsPagingEnabled { get; private set; }
    
    protected SpecificationDashboardEntity(Expression<Func<TEntity, bool>> criteria)
    {
        Criteria = criteria;
        AddInclude(e => e.User);
    }

    public IReadOnlyList<Expression<Func<TEntity, object>>> Includes => _includes;
    public IReadOnlyList<string> IncludeStrings => _includeStrings;
    
    protected void AddInclude(Expression<Func<TEntity, object>> includeExpression) =>
        _includes.Add(includeExpression);

    protected void AddInclude(string includeString) =>
        _includeStrings.Add(includeString);

    protected void ApplyPaging(int skip, int take)
    {
        Skip = skip;
        Take = take;
        IsPagingEnabled = true;
    }

    protected void ApplyOrderBy(Expression<Func<TEntity, object>>? orderByExpression) =>
        OrderBy = orderByExpression;

    protected void ApplyOrderByDescending(Expression<Func<TEntity, object>>? orderByDescendingExpression) =>
        OrderByDescending = orderByDescendingExpression;

    public abstract class BuilderDashboardEntity
    {
        protected Expression<Func<TEntity, bool>> criteria = e => true;
        
        public abstract SpecificationDashboardEntity<TEntity, TBuilder> Build();

        public BuilderDashboardEntity WithId(int id) => WithIdRange([id]);

        public BuilderDashboardEntity WithIdRange(IReadOnlyCollection<int> ids)
        {
            foreach (int id in ids)
            {
                ValidateNotNegative(id);
            }

            criteria = criteria.And(e => ids.Contains(e.Id));
            return this;
        }

        protected static void ValidateNotNegative(decimal value)
        {
            if (value < 0)
            {
                throw new ArgumentException("Value must not be negative", nameof(value));
            }
        }

        protected static void ValidateRange<T>(T min, T max) where T : IComparable
        {
            if (min.CompareTo(max) > 0)
            {
                throw new ArgumentException($"Min ({min}) value must not be greater than max ({max}) value");
            }
        }
    }
}