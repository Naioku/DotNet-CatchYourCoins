using System.Linq.Expressions;
using Domain.Dashboard.Entities.Expenses;

namespace Domain.Dashboard.Specifications.Expenses;

public sealed class SpecificationExpensePaymentMethod(Expression<Func<ExpensePaymentMethod, bool>> criteria)
    : SpecificationFinancialCategory<
        ExpensePaymentMethod,
        SpecificationExpensePaymentMethod.BuilderExpensePaymentMethod
    >(criteria)
{
    public static BuilderExpensePaymentMethod GetBuilder() => new();
    
    public class BuilderExpensePaymentMethod : BuilderFinancialCategory
    {
        internal BuilderExpensePaymentMethod() {}

        public override SpecificationExpensePaymentMethod Build() => new(criteria);
    }
}