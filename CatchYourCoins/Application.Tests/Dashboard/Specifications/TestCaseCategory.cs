using Application.Tests.TestObjects.Entity;

namespace Application.Tests.Dashboard.Specifications;

public class TestCaseCategoryBase
{
    public required TestObjCategory? CheckedCategory { get; init; }
    public required bool ExpectedResult { get; init; }
    public required string TestDescription { get; init; }

    public override string ToString() => TestDescription;
}

public class TestCaseCategoryName : TestCaseCategoryBase
{
    public required string? SearchedCategoryName { get; init; }
}

public class TestCaseCategoryId : TestCaseCategoryBase
{
    public required int? SearchedCategoryId { get; set; }
}