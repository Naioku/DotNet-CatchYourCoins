using System.Linq.Expressions;
using Domain.Dashboard.Entities.Expenses;

namespace Domain.Dashboard.Specifications.Expenses;

public sealed class SpecificationExpenseCategory(Expression<Func<ExpenseCategory, bool>> criteria)
    : SpecificationFinancialCategory<
        ExpenseCategory,
        SpecificationExpenseCategory.BuilderExpenseCategory
    >(criteria)
{
    public static BuilderExpenseCategory GetBuilder() => new();
    
    public class BuilderExpenseCategory : BuilderFinancialCategory
    {
        internal BuilderExpenseCategory() {}

        public override SpecificationExpenseCategory Build() => new(criteria);
    }
}