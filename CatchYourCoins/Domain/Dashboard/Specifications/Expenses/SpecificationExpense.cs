using System.Linq.Expressions;
using Domain.Dashboard.Entities.Expenses;
using LinqKit;

namespace Domain.Dashboard.Specifications.Expenses;

public sealed class SpecificationExpense
    : SpecificationFinancialOperation<
        Expense,
        SpecificationExpense.BuilderExpense,
        ExpenseCategory
    >
{
    private SpecificationExpense(Expression<Func<Expense, bool>> criteria) : base(criteria)
    {
        AddInclude(e => e.PaymentMethod);
    }

    public static BuilderExpense GetBuilder() => new();
    
    public class BuilderExpense : BuilderFinancialOperation
    {
        internal BuilderExpense() {}

        public override SpecificationExpense Build() => new(criteria);
    
        public BuilderExpense WithPaymentMethod(int id)
        {
            ValidateNotNegative(id);

            criteria = criteria.And(e => e.PaymentMethodId == id);
            return this;
        }

        public BuilderExpense WithPaymentMethod(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                criteria = criteria.And(e => e.PaymentMethod == null);
            }
            else
            {
                criteria = criteria.And(e =>
                    e.PaymentMethod != null &&
                    e.PaymentMethod.Name.Contains(name, StringComparison.CurrentCultureIgnoreCase)
                );
            }
        
            return this;
        }
    }
}