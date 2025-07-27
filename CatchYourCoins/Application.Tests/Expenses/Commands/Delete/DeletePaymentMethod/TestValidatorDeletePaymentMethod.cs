using Application.Expenses.Commands.Delete;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Expenses.Commands.Delete.DeletePaymentMethod;

[TestSubject(typeof(ValidatorDeleteCategory))]
public class TestValidatorDeletePaymentMethod : TestValidatorBase<ValidatorDeletePaymentMethod, CommandDeletePaymentMethod>
{
    [Fact]
    public void Validate_AllValidData_NoError() =>
        AssertSuccess(new CommandDeletePaymentMethod { Id = 1 });

    [Fact]
    public void Validate_InvalidId_Error() =>
        AssertFailure(new CommandDeletePaymentMethod { Id = -1 });
}