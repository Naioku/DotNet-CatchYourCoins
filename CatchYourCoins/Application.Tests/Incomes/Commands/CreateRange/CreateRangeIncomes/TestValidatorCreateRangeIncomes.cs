using System;
using Application.DTOs.InputDTOs.Incomes;
using Application.Incomes.Commands.CreateRange;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Incomes.Commands.CreateRange.CreateRangeIncomes;

[TestSubject(typeof(ValidatorCreateRangeIncomes))]
public class TestValidatorCreateRangeIncomes
    : TestValidatorBase<ValidatorCreateRangeIncomes, CommandCreateRangeIncomes>
{
    [Fact]
    public void Validate_AllValidData_NoError() =>
        AssertSuccess(new CommandCreateRangeIncomes
        {
            Data = [
                new InputDTOIncome
                {
                    Amount = 100,
                    Date = DateTime.Now,
                    Description = "Test1",
                    CategoryId = 1,
                },
                new InputDTOIncome
                {
                    Amount = 200,
                    Date = DateTime.Now,
                    Description = "Test1",
                    CategoryId = 2,
                },
            ],
        });

    [Fact]
    public void Validate_MinimumValidData_NoError() =>
        AssertSuccess(new CommandCreateRangeIncomes
        {
            Data = [
                new InputDTOIncome
                {
                    Amount = 100,
                    Date = DateTime.Now,
                },
                new InputDTOIncome
                {
                    Amount = 200,
                    Date = DateTime.Now,
                },
            ],
        });

    [Fact]
    public void Validate_InvalidAmount_Error() =>
        AssertFailure(new CommandCreateRangeIncomes
        {
            Data = [
                new InputDTOIncome
                {
                    Amount = -100,
                    Date = DateTime.Now,
                },
                new InputDTOIncome
                {
                    Amount = 200,
                    Date = DateTime.Now,
                },
            ],
        });
}