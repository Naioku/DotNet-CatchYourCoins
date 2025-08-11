using System.Linq.Expressions;
using Domain.Dashboard.Entities;
using LinqKit;

namespace Domain.Dashboard.Specifications;

public abstract class SpecificationFinancialCategory<TEntity, TBuilder>(Expression<Func<TEntity, bool>> criteria)
    : SpecificationDashboardEntity<TEntity, TBuilder>(criteria)
    where TEntity : FinancialCategory
    where TBuilder : SpecificationFinancialCategory<TEntity, TBuilder>.BuilderFinancialCategory
{
    public abstract class BuilderFinancialCategory : BuilderDashboardEntity
    {
        public BuilderFinancialCategory WithName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name must not be null, empty or whitespace", nameof(name));
            }

            criteria = criteria.And(e => e.Name.Contains(name, StringComparison.CurrentCultureIgnoreCase));
            return this;
        }

        public BuilderFinancialCategory WithLimit(decimal limit) =>
            WithLimitRange(limit, limit);

        public BuilderFinancialCategory WithLimitRange(
            decimal min,
            decimal max)
        {
            ValidateNotNegative(min);
            ValidateNotNegative(max);
            ValidateRange(min, max);

            criteria = criteria.And(e =>
                e.Limit >= min &&
                e.Limit <= max
            );
            return this;
        }
    }
}