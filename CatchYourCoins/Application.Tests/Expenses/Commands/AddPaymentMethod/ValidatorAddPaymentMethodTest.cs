using Application.Expenses.Commands;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Expenses.Commands.AddPaymentMethod;

[TestSubject(typeof(ValidatorAddPaymentMethod))]
public class ValidatorAddPaymentMethodTest : ValidatorTestBase<ValidatorAddPaymentMethod, CommandAddPaymentMethod>
{
    [Fact]
    public void Validate_AllValidData_NoError() =>
        AssertSuccess(new CommandAddPaymentMethod
        {
            Name = "Test",
            Limit = 1000
        });

    [Fact]
    public void Validate_MinimalValidData_NoError() =>
        AssertSuccess(new CommandAddPaymentMethod
        {
            Name = "Test",
        });
    
    [Fact]
    public void Validate_EmptyName_Error()
    {
        AssertFailure(new CommandAddPaymentMethod
        {
            Name = "",
            Limit = 1000
        });
    }
}