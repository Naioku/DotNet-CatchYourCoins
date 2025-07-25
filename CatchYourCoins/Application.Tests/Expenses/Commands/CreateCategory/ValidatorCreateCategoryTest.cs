using Application.Expenses.Commands;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Expenses.Commands.CreateCategory;

[TestSubject(typeof(ValidatorCreateCategory))]
public class ValidatorCreateCategoryTest : ValidatorTestBase<ValidatorCreateCategory, CommandCreateCategory>
{
    [Fact]
    public void Validate_AllValidData_NoError() =>
        AssertSuccess(new CommandCreateCategory
        {
            Name = "Test",
            Limit = 1000
        });

    [Fact]
    public void Validate_MinimalValidData_NoError() =>
        AssertSuccess(new CommandCreateCategory
        {
            Name = "Test"
        });

    [Fact]
    public void Validate_EmptyName_Error() =>
        AssertFailure(new CommandCreateCategory
        {
            Name = "",
            Limit = 1000
        });
}