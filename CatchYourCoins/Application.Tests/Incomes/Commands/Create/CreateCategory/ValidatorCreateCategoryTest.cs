using Application.DTOs.InputDTOs.Incomes;
using Application.Incomes.Commands.Create;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Incomes.Commands.Create.CreateCategory;

[TestSubject(typeof(ValidatorCreateCategory))]
public class ValidatorCreateCategoryTest : TestValidatorBase<ValidatorCreateCategory, CommandCreateCategory>
{
    [Fact]
    public void Validate_AllValidData_NoError() =>
        AssertSuccess(new CommandCreateCategory
        {
            Data = new InputDTOIncomeCategory
            {
                Name = "Test",
                Limit = 1000
            }
        });

    [Fact]
    public void Validate_MinimalValidData_NoError() =>
        AssertSuccess(new CommandCreateCategory
        {
            Data = new InputDTOIncomeCategory
            {
                Name = "Test"
            }
        });

    [Fact]
    public void Validate_EmptyName_Error() =>
        AssertFailure(new CommandCreateCategory
        {
            Data = new InputDTOIncomeCategory
            {
                Name = "",
                Limit = 1000
            }
        });
}