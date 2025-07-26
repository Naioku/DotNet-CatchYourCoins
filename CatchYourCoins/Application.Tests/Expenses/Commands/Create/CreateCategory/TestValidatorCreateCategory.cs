using Application.Expenses.Commands.Create;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Expenses.Commands.Create.CreateCategory;

[TestSubject(typeof(Application.Expenses.Commands.Create.TestValidatorCreateCategory))]
public class TestValidatorCreateCategory : TestValidatorBase<Application.Expenses.Commands.Create.TestValidatorCreateCategory, CommandCreateCategory>
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