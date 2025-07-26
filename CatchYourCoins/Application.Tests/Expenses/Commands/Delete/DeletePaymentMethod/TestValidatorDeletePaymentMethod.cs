using Application.Expenses.Commands.Delete;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Expenses.Commands.Delete.DeletePaymentMethod;

[TestSubject(typeof(TestValidatorDeleteCategory))]
public class TestValidatorDeletePaymentMethod : TestValidatorBase<Application.Expenses.Commands.Delete.TestValidatorDeletePaymentMethod, CommandDeletePaymentMethod>
{
    [Fact]
    public void DeletePaymentMethod_AllValidData_NoError() =>
        AssertSuccess(new CommandDeletePaymentMethod { Id = 1 });

    [Fact]
    public void DeletePaymentMethod_InvalidId_Error() =>
        AssertFailure(new CommandDeletePaymentMethod { Id = -1 });
}