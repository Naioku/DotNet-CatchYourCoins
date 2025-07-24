using Application.Expenses.Commands;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Expenses.Commands.DeleteExpense;

[TestSubject(typeof(ValidatorDeleteExpense))]
public class ValidatorDeleteExpenseTest : ValidatorTestBase<ValidatorDeleteExpense, CommandDeleteExpense>
{
    [Fact]
    public void DeletePaymentMethod_AllValidData_NoError() =>
        AssertSuccess(new CommandDeleteExpense { Id = 1 });

    [Fact]
    public void DeletePaymentMethod_InvalidId_Error() =>
        AssertFailure(new CommandDeleteExpense { Id = -1 });
}