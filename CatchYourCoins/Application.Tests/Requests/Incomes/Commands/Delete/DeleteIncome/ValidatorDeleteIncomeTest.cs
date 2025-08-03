using Application.Requests.Incomes.Commands.Delete;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Requests.Incomes.Commands.Delete.DeleteIncome;

[TestSubject(typeof(ValidatorDeleteIncome))]
public class ValidatorDeleteIncomeTest : TestValidatorBase<ValidatorDeleteIncome, CommandDeleteIncome>
{
    [Fact]
    public void Validate_AllValidData_NoError() =>
        AssertSuccess(new CommandDeleteIncome { Id = 1 });

    [Fact]
    public void Validate_InvalidId_Error() =>
        AssertFailure(new CommandDeleteIncome { Id = -1 });
}