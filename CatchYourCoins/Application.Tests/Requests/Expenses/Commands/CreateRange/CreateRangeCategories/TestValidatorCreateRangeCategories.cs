using Application.DTOs.InputDTOs.Expenses;
using Application.Requests.Expenses.Commands.CreateRange;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Requests.Expenses.Commands.CreateRange.CreateRangeCategories;

[TestSubject(typeof(ValidatorCreateRangeCategories))]
public class TestValidatorCreateRangeCategories
    : TestValidatorBase<ValidatorCreateRangeCategories, CommandCreateRangeCategories>
{
    [Fact]
    public void Validate_AllValidData_NoError() =>
        AssertSuccess(new CommandCreateRangeCategories
        {
            Data = [
                new InputDTOExpenseCategory
                {
                    Name = "Test1",
                    Limit = 1000
                },
                new InputDTOExpenseCategory
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
                new InputDTOExpenseCategory
                {
                    Name = "Test1",
                },
                new InputDTOExpenseCategory
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
                new InputDTOExpenseCategory
                {
                    Name = "",
                    Limit = 1000
                },
                new InputDTOExpenseCategory
                {
                    Name = "",
                    Limit = 2000
                },
            ],
        });
}