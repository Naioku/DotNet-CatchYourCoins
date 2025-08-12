using System.Linq.Expressions;
using Domain.Dashboard.Entities.Expenses;

namespace Domain.Dashboard.Specifications.Expenses;

public sealed class SpecificationExpenseCategory : SpecificationFinancialCategory<
        ExpenseCategory,
        SpecificationExpenseCategory.BuilderExpenseCategory
    >
{
    private SpecificationExpenseCategory(Expression<Func<ExpenseCategory, bool>> criteria) : base(criteria) {}

    public static BuilderExpenseCategory GetBuilder() => new();
    
    public class BuilderExpenseCategory : BuilderFinancialCategory
    {
        internal BuilderExpenseCategory() {}

        public override SpecificationExpenseCategory Build() => new(criteria);
    }
}