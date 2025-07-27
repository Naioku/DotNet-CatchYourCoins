using System;
using Application.Expenses.Commands.Create;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Expenses.Commands.Create.CreateExpense;

[TestSubject(typeof(ValidatorCreateExpense))]
public class TestValidatorCreateExpense : TestValidatorBase<ValidatorCreateExpense, CommandCreateExpense>
{
    [Fact]
    public void Validate_AllValidData_NoError() =>
        AssertSuccess(new CommandCreateExpense
        {
            Amount = 100,
            Date = DateTime.Now,
            Description = "Test",
            CategoryId = 1,
            PaymentMethodId = 1,
        });

    [Fact]
    public void Validate_MinimumValidData_NoError() =>
        AssertSuccess(new CommandCreateExpense
        {
            Amount = 100,
            Date = DateTime.Now,
        });

    [Fact]
    public void Validate_InvalidAmount_Error() =>
        AssertFailure(new CommandCreateExpense
        {
            Amount = -100,
            Date = DateTime.Now,
        });
}