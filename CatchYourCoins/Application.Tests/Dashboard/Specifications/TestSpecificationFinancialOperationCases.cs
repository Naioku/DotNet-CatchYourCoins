using System;
using Application.Tests.TestObjects.Entity;
using Xunit;

namespace Application.Tests.Dashboard.Specifications;

public partial class TestSpecificationFinancialOperation
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
    
    public static TheoryData<TestCaseCategoryName> TestCasesCategoryName =>
    [
        new()
        {
            SearchedCategoryName = "Test1",
            CheckedCategory = new TestObjCategory
            {
                Id = 1,
                Name = "Test1",
                UserId = Guid.Empty,
            },
            ExpectedResult = true,
            TestDescription = "Object with category found (specific value)"
        },
        new()
        {
            SearchedCategoryName = "",
            CheckedCategory = null,
            ExpectedResult = true,
            TestDescription = "Object with category found (empty string)"
        },
        new()
        {
            SearchedCategoryName = null,
            CheckedCategory = null,
            ExpectedResult = true,
            TestDescription = "Object with category found (null)"
        },
        new()
        {
            SearchedCategoryName = "Test1",
            CheckedCategory = new TestObjCategory
            {
                Id = 2,
                Name = "Test2",
                UserId = Guid.Empty,
            },
            ExpectedResult = false,
            TestDescription = "Object with category not found (specific value)"
        },
        new()
        {
            SearchedCategoryName = "",
            CheckedCategory = new TestObjCategory
            {
                Id = 2,
                Name = "Test2",
                UserId = Guid.Empty,
            },
            ExpectedResult = false,
            TestDescription = "Object with category not found (empty string)"
        },
        new()
        {
            SearchedCategoryName = null,
            CheckedCategory = new TestObjCategory
            {
                Id = 2,
                Name = "Test2",
                UserId = Guid.Empty,
            },
            ExpectedResult = false,
            TestDescription = "Object with category not found (null)"
        },
    ];

    public static TheoryData<TestCaseCategoryId> TestCasesCategoryId =>
    [
        new()
        {
            SearchedCategoryId = 1,
            CheckedCategory = new TestObjCategory
            {
                Id = 1,
                Name = "Test1",
                UserId = Guid.Empty,
            },
            ExpectedResult = true,
            TestDescription = "Object with category found (specific value)"
        },
        new()
        {
            SearchedCategoryId = null,
            CheckedCategory = null,
            ExpectedResult = true,
            TestDescription = "Object with category found (null)"
        },
        new()
        {
            SearchedCategoryId = 1,
            CheckedCategory = new TestObjCategory
            {
                Id = 2,
                Name = "Test2",
                UserId = Guid.Empty,
            },
            ExpectedResult = false,
            TestDescription = "Object with category not found (specific value)"
        },
        new()
        {
            SearchedCategoryId = 1,
            CheckedCategory = null,
            ExpectedResult = false,
            TestDescription = "Object with category not found (null)"
        },
    ];
}