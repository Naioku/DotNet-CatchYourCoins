using System;
using Domain.Dashboard.Entities.Expenses;
using Xunit;

namespace Application.Tests.Dashboard.Specifications.Expenses;

public partial class TestSpecificationExpense
{
    public static TheoryData<TestCasePaymentMethodName> TestCasesPaymentMethodName =>
    [
        new()
        {
            SearchedPaymentMethodName = "Test1",
            CheckedPaymentMethod = new ExpensePaymentMethod
            {
                Id = 1,
                Name = "Test1",
                UserId = Guid.Empty,
            },
            ExpectedResult = true,
            TestDescription = "Object with payment method found (specific value)"
        },
        new()
        {
            SearchedPaymentMethodName = "",
            CheckedPaymentMethod = null,
            ExpectedResult = true,
            TestDescription = "Object with payment method found (empty string)"
        },
        new()
        {
            SearchedPaymentMethodName = null,
            CheckedPaymentMethod = null,
            ExpectedResult = true,
            TestDescription = "Object with payment method found (null)"
        },
        new()
        {
            SearchedPaymentMethodName = "Test1",
            CheckedPaymentMethod = new ExpensePaymentMethod
            {
                Id = 2,
                Name = "Test2",
                UserId = Guid.Empty,
            },
            ExpectedResult = false,
            TestDescription = "Object with payment method not found (specific value)"
        },
        new()
        {
            SearchedPaymentMethodName = "",
            CheckedPaymentMethod = new ExpensePaymentMethod
            {
                Id = 2,
                Name = "Test2",
                UserId = Guid.Empty,
            },
            ExpectedResult = false,
            TestDescription = "Object with payment method not found (empty string)"
        },
        new()
        {
            SearchedPaymentMethodName = null,
            CheckedPaymentMethod = new ExpensePaymentMethod
            {
                Id = 2,
                Name = "Test2",
                UserId = Guid.Empty,
            },
            ExpectedResult = false,
            TestDescription = "Object with payment method not found (null)"
        },
    ];

    public static TheoryData<TestCasePaymentMethodId> TestCasesPaymentMethodId =>
    [
        new()
        {
            SearchedPaymentMethodId = 1,
            CheckedPaymentMethod = new ExpensePaymentMethod
            {
                Id = 1,
                Name = "Test1",
                UserId = Guid.Empty,
            },
            ExpectedResult = true,
            TestDescription = "Object with payment method found (specific value)"
        },
        new()
        {
            SearchedPaymentMethodId = null,
            CheckedPaymentMethod = null,
            ExpectedResult = true,
            TestDescription = "Object with payment method found (null)"
        },
        new()
        {
            SearchedPaymentMethodId = 1,
            CheckedPaymentMethod = new ExpensePaymentMethod
            {
                Id = 2,
                Name = "Test2",
                UserId = Guid.Empty,
            },
            ExpectedResult = false,
            TestDescription = "Object with payment method not found (specific value)"
        },
        new()
        {
            SearchedPaymentMethodId = 1,
            CheckedPaymentMethod = null,
            ExpectedResult = false,
            TestDescription = "Object with payment method not found (null)"
        },
    ];
}