namespace Application.Tests.Dashboard.Specifications;

public class TestCaseAmountRangeException
{
    public required decimal AmountMin { get; init; }
    public required decimal AmountMax { get; init; }
    public required string TestDescription { get; init; }

    public override string ToString() => TestDescription;
}