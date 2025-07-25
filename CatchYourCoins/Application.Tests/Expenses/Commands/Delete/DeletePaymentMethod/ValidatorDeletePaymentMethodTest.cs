using Application.Expenses.Commands.Delete;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Expenses.Commands.Delete.DeletePaymentMethod;

[TestSubject(typeof(ValidatorDeleteCategory))]
public class ValidatorDeletePaymentMethodTest : ValidatorTestBase<ValidatorDeletePaymentMethod, CommandDeletePaymentMethod>
{
    [Fact]
    public void DeletePaymentMethod_AllValidData_NoError() =>
        AssertSuccess(new CommandDeletePaymentMethod { Id = 1 });

    [Fact]
    public void DeletePaymentMethod_InvalidId_Error() =>
        AssertFailure(new CommandDeletePaymentMethod { Id = -1 });
}