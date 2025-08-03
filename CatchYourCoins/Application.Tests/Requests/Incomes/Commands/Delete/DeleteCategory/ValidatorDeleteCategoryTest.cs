using Application.Requests.Incomes.Commands.Delete;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Requests.Incomes.Commands.Delete.DeleteCategory;

[TestSubject(typeof(ValidatorDeleteCategory))]
public class ValidatorDeleteCategoryTest : TestValidatorBase<ValidatorDeleteCategory, CommandDeleteCategory>
{
    [Fact]
    public void Validate_AllValidData_NoError() =>
        AssertSuccess(new CommandDeleteCategory { Id = 1 });

    [Fact]
    public void Validate_InvalidId_Error() =>
        AssertFailure(new CommandDeleteCategory { Id = -1 });
}