using Application.Expenses.Commands.Delete;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Expenses.Commands.Delete.DeleteExpense;

[TestSubject(typeof(ValidatorDeleteExpense))]
public class TestValidatorDeleteExpense : TestValidatorBase<ValidatorDeleteExpense, CommandDeleteExpense>
{
    [Fact]
    public void DeletePaymentMethod_AllValidData_NoError() =>
        AssertSuccess(new CommandDeleteExpense { Id = 1 });

    [Fact]
    public void DeletePaymentMethod_InvalidId_Error() =>
        AssertFailure(new CommandDeleteExpense { Id = -1 });
}