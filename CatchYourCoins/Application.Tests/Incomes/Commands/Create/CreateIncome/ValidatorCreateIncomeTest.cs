using System;
using Application.DTOs.InputDTOs.Incomes;
using Application.Incomes.Commands.Create;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Incomes.Commands.Create.CreateIncome;

[TestSubject(typeof(ValidatorCreateIncome))]
public class ValidatorCreateIncomeTest : TestValidatorBase<ValidatorCreateIncome, CommandCreateIncome>
{
    [Fact]
    public void Validate_AllValidData_NoError() =>
        AssertSuccess(new CommandCreateIncome
        {
            Data = new InputDTOIncome
            {
                Amount = 100,
                Date = DateTime.Now,
                Description = "Test",
                CategoryId = 1,
            }
        });

    [Fact]
    public void Validate_MinimumValidData_NoError() =>
        AssertSuccess(new CommandCreateIncome
        {
            Data = new InputDTOIncome
            {
                Amount = 100,
                Date = DateTime.Now,
            }
        });

    [Fact]
    public void Validate_InvalidAmount_Error() =>
        AssertFailure(new CommandCreateIncome
        {
            Data = new InputDTOIncome
            {
                Amount = -100,
                Date = DateTime.Now,
            }
        });
}