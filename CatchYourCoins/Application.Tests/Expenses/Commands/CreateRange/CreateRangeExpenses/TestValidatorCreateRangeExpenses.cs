using System;
using Application.DTOs.InputDTOs.Expenses;
using Application.Expenses.Commands.CreateRange;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Expenses.Commands.CreateRange.CreateRangeExpenses;

[TestSubject(typeof(ValidatorCreateRangeExpenses))]
public class TestValidatorCreateRangeExpenses
    : TestValidatorBase<ValidatorCreateRangeExpenses, CommandCreateRangeExpenses>
{
    [Fact]
    public void Validate_AllValidData_NoError() =>
        AssertSuccess(new CommandCreateRangeExpenses
        {
            Data = [
                new InputDTOExpense
                {
                    Amount = 100,
                    Date = DateTime.Now,
                    Description = "Test1",
                    CategoryId = 1,
                    PaymentMethodId = 1
                },
                new InputDTOExpense
                {
                    Amount = 200,
                    Date = DateTime.Now,
                    Description = "Test1",
                    CategoryId = 2,
                    PaymentMethodId = 2
                },
            ],
        });

    [Fact]
    public void Validate_MinimumValidData_NoError() =>
        AssertSuccess(new CommandCreateRangeExpenses
        {
            Data = [
                new InputDTOExpense
                {
                    Amount = 100,
                    Date = DateTime.Now,
                },
                new InputDTOExpense
                {
                    Amount = 200,
                    Date = DateTime.Now,
                },
            ],
        });

    [Fact]
    public void Validate_InvalidAmount_Error() =>
        AssertFailure(new CommandCreateRangeExpenses
        {
            Data = [
                new InputDTOExpense
                {
                    Amount = -100,
                    Date = DateTime.Now,
                },
                new InputDTOExpense
                {
                    Amount = 200,
                    Date = DateTime.Now,
                },
            ],
        });
}