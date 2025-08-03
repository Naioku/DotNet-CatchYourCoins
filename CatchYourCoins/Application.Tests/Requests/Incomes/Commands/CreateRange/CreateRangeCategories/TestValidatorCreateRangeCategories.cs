using Application.DTOs.InputDTOs.Incomes;
using Application.Requests.Incomes.Commands.CreateRange;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Requests.Incomes.Commands.CreateRange.CreateRangeCategories;

[TestSubject(typeof(ValidatorCreateRangeCategories))]
public class TestValidatorCreateRangeCategories
    : TestValidatorBase<ValidatorCreateRangeCategories, CommandCreateRangeCategories>
{
    [Fact]
    public void Validate_AllValidData_NoError() =>
        AssertSuccess(new CommandCreateRangeCategories
        {
            Data = [
                new InputDTOIncomeCategory
                {
                    Name = "Test1",
                    Limit = 1000
                },
                new InputDTOIncomeCategory
                {
                    Name = "Test2",
                    Limit = 2000
                },
            ],
            
        });

    [Fact]
    public void Validate_MinimalValidData_NoError() =>
        AssertSuccess(new CommandCreateRangeCategories
        {
            Data = [
                new InputDTOIncomeCategory
                {
                    Name = "Test1",
                },
                new InputDTOIncomeCategory
                {
                    Name = "Test2",
                },
            ],
        });

    [Fact]
    public void Validate_EmptyName_Error() =>
        AssertFailure(new CommandCreateRangeCategories
        {
            Data = [
                new InputDTOIncomeCategory
                {
                    Name = "",
                    Limit = 1000
                },
                new InputDTOIncomeCategory
                {
                    Name = "",
                    Limit = 2000
                },
            ],
        });
}