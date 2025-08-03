using Application.Requests.Expenses.Commands.Delete;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Requests.Expenses.Commands.Delete.DeleteExpense;

[TestSubject(typeof(ValidatorDeleteExpense))]
public class TestValidatorDeleteExpense : TestValidatorBase<ValidatorDeleteExpense, CommandDeleteExpense>
{
    [Fact]
    public void Validate_AllValidData_NoError() =>
        AssertSuccess(new CommandDeleteExpense { Id = 1 });

    [Fact]
    public void Validate_InvalidId_Error() =>
        AssertFailure(new CommandDeleteExpense { Id = -1 });
}