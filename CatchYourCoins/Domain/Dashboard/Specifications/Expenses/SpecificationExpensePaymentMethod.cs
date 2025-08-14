using System.Linq.Expressions;
using Domain.Dashboard.Entities.Expenses;

namespace Domain.Dashboard.Specifications.Expenses;

public sealed class SpecificationExpensePaymentMethod : SpecificationFinancialCategory<
        ExpensePaymentMethod,
        SpecificationExpensePaymentMethod.BuilderExpensePaymentMethod
    >
{
    private SpecificationExpensePaymentMethod(Expression<Func<ExpensePaymentMethod, bool>> criteria) : base(criteria) {}

    public static BuilderExpensePaymentMethod GetBuilder() => new();
    
    public class BuilderExpensePaymentMethod : BuilderFinancialCategory
    {
        internal BuilderExpensePaymentMethod() {}

        public override SpecificationExpensePaymentMethod Build() => new(criteria);
    }
}