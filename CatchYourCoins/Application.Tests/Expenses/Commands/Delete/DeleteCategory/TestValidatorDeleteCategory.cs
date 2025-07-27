using Application.Expenses.Commands.Delete;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Expenses.Commands.Delete.DeleteCategory;

[TestSubject(typeof(ValidatorDeleteCategory))]
public class TestValidatorDeleteCategory : TestValidatorBase<ValidatorDeleteCategory, CommandDeleteCategory>
{
    [Fact]
    public void DeletePaymentMethod_AllValidData_NoError() =>
        AssertSuccess(new CommandDeleteCategory { Id = 1 });

    [Fact]
    public void DeletePaymentMethod_InvalidId_Error() =>
        AssertFailure(new CommandDeleteCategory { Id = -1 });
}