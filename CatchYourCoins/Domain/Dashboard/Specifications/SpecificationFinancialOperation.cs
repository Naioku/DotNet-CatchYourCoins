using System.Linq.Expressions;
using Domain.Dashboard.Entities;
using LinqKit;

namespace Domain.Dashboard.Specifications;

public abstract class SpecificationFinancialOperation<TEntity, TBuilder, TCategory>
    : SpecificationDashboardEntity<TEntity, TBuilder>
    where TEntity : FinancialOperation<TCategory>
    where TBuilder : SpecificationFinancialOperation<TEntity, TBuilder, TCategory>.BuilderFinancialOperation
    where TCategory : FinancialCategory
{
    protected SpecificationFinancialOperation(Expression<Func<TEntity, bool>> criteria) : base(criteria)
    {
        AddInclude(e => e.User);
        AddInclude(o => o.Category);
    }

    public abstract class BuilderFinancialOperation : BuilderDashboardEntity
    {
        public BuilderFinancialOperation WithAmount(decimal amount) =>
            WithAmountRange(amount, amount);

        public BuilderFinancialOperation WithAmountRange(
            decimal min,
            decimal max)
        {
            ValidateNotNegative(min);
            ValidateNotNegative(max);
            ValidateRange(min, max);

            criteria = criteria.And(e =>
                e.Amount >= min &&
                e.Amount <= max
            );
            return this;
        }

        public BuilderFinancialOperation WithDate(DateTime dateTime) =>
            WithDateRange(dateTime, dateTime);

        public BuilderFinancialOperation WithDateRange(
            DateTime min,
            DateTime max)
        {
            ValidateRange(min, max);

            criteria = criteria.And(e =>
                e.Date >= min &&
                e.Date <= max
            );
            return this;
        }

        public BuilderFinancialOperation WithDescription(string? description)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                criteria = criteria.And(e => e.Description == null);
            }
            else
            {
                criteria = criteria.And(e =>
                    e.Description != null &&
                    e.Description.Contains(description, StringComparison.CurrentCultureIgnoreCase)
                );
            }

            return this;
        }

        public BuilderFinancialOperation WithCategory(int? id)
        {
            if (id != null)
            {
                ValidateNotNegative(id.Value);
            }

            criteria = criteria.And(e => e.CategoryId == id);
            return this;
        }

        public BuilderFinancialOperation WithCategory(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                criteria = criteria.And(e => e.Category == null);
            }
            else
            {
                criteria = criteria.And(e =>
                    e.Category != null &&
                    e.Category.Name.Contains(name, StringComparison.CurrentCultureIgnoreCase)
                );
            }

            return this;
        }
    }
}