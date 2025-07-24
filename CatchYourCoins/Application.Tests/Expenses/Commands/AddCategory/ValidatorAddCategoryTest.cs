using Application.Expenses.Commands;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Expenses.Commands.AddCategory;

[TestSubject(typeof(ValidatorAddCategory))]
public class ValidatorAddCategoryTest : ValidatorTestBase<ValidatorAddCategory, CommandAddCategory>
{
    [Fact]
    public void Validate_AllValidData_NoError() =>
        AssertSuccess(new CommandAddCategory
        {
            Name = "Test",
            Limit = 1000
        });

    [Fact]
    public void Validate_MinimalValidData_NoError() =>
        AssertSuccess(new CommandAddCategory
        {
            Name = "Test"
        });

    [Fact]
    public void Validate_EmptyName_Error() =>
        AssertFailure(new CommandAddCategory
        {
            Name = "",
            Limit = 1000
        });
}