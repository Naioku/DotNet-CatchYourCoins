using Xunit;

namespace Application.Tests.Dashboard.Specifications;

public partial class TestSpecificationFinancialCategory
{
    public static TheoryData<TestCaseAmountRangeException> TestCasesAmountRangeException =>
    [
        new()
        {
            AmountMin = -10,
            AmountMax = 5,
            TestDescription = "Negative min amount"
        },
        new()
        {
            AmountMin = 10,
            AmountMax = -5,
            TestDescription = "Negative max amount"
        },
        new()
        {
            AmountMin = 10,
            AmountMax = 5,
            TestDescription = "Min > Max"
        }
    ];
}