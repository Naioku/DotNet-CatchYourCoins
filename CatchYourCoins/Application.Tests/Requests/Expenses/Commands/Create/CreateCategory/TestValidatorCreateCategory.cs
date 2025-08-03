using Application.DTOs.InputDTOs.Expenses;
using Application.Requests.Expenses.Commands.Create;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Requests.Expenses.Commands.Create.CreateCategory;

[TestSubject(typeof(ValidatorCreateCategory))]
public class TestValidatorCreateCategory : TestValidatorBase<ValidatorCreateCategory, CommandCreateCategory>
{
    [Fact]
    public void Validate_AllValidData_NoError() =>
        AssertSuccess(new CommandCreateCategory
        {
            Data = new InputDTOExpenseCategory
            {
                Name = "Test",
                Limit = 1000
            }
        });

    [Fact]
    public void Validate_MinimalValidData_NoError() =>
        AssertSuccess(new CommandCreateCategory
        {
            Data = new InputDTOExpenseCategory
            {
                Name = "Test",
            }
        });

    [Fact]
    public void Validate_EmptyName_Error() =>
        AssertFailure(new CommandCreateCategory
        {
            Data = new InputDTOExpenseCategory
            {
                Name = "",
                Limit = 1000
            }
        });
}