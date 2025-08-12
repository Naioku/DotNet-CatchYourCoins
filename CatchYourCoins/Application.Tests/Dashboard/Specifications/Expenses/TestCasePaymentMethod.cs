using Domain.Dashboard.Entities.Expenses;

namespace Application.Tests.Dashboard.Specifications.Expenses;

public class TestCasePaymentMethodBase
{
    public required ExpensePaymentMethod? CheckedPaymentMethod { get; init; }
    public required bool ExpectedResult { get; init; }
    public required string TestDescription { get; init; }

    public override string ToString() => TestDescription;
}

public class TestCasePaymentMethodName : TestCasePaymentMethodBase
{
    public required string? SearchedPaymentMethodName { get; init; }
}

public class TestCasePaymentMethodId : TestCasePaymentMethodBase
{
    public required int? SearchedPaymentMethodId { get; set; }
}