using System.Linq.Expressions;
using Domain.Dashboard.Entities.Incomes;

namespace Domain.Dashboard.Specifications.Incomes;

public sealed class SpecificationIncomeCategory(Expression<Func<IncomeCategory, bool>> criteria)
    : SpecificationFinancialCategory<
        IncomeCategory,
        SpecificationIncomeCategory.BuilderIncomeCategory
    >(criteria)
{
    public static BuilderIncomeCategory GetBuilder() => new();
    
    public class BuilderIncomeCategory : BuilderFinancialCategory
    {
        internal BuilderIncomeCategory() {}

        public override SpecificationIncomeCategory Build() => new(criteria);
    }
}