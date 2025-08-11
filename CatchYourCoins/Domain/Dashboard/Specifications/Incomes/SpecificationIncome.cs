using System.Linq.Expressions;
using Domain.Dashboard.Entities.Incomes;

namespace Domain.Dashboard.Specifications.Incomes;

public sealed class SpecificationIncome(Expression<Func<Income, bool>> criteria)
    : SpecificationFinancialOperation<
        Income,
        SpecificationIncome.BuilderIncome,
        IncomeCategory
    >(criteria)
{
    public static BuilderIncome GetBuilder() => new();
    
    public class BuilderIncome : BuilderFinancialOperation
    {
        internal BuilderIncome() {}

        public override SpecificationIncome Build() => new(criteria);
    }
}