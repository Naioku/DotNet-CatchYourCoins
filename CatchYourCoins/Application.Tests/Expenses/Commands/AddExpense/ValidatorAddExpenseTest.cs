using System;
using Application.Expenses.Commands;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Expenses.Commands.AddExpense;

[TestSubject(typeof(ValidatorAddExpense))]
public class ValidatorAddExpenseTest : ValidatorTestBase<ValidatorAddExpense, CommandAddExpense>
{
    [Fact]
    public void Validate_AllValidData_NoError() =>
        AssertSuccess(new CommandAddExpense
        {
            Amount = 100,
            Date = DateTime.Now,
            Description = "Test",
            CategoryId = 1,
            PaymentMethodId = 1,
        });

    [Fact]
    public void Validate_MinimumValidData_NoError() =>
        AssertSuccess(new CommandAddExpense
        {
            Amount = 100,
            Date = DateTime.Now,
        });

    [Fact]
    public void Validate_InvalidAmount_Error() =>
        AssertFailure(new CommandAddExpense
        {
            Amount = -100,
            Date = DateTime.Now,
        });
}