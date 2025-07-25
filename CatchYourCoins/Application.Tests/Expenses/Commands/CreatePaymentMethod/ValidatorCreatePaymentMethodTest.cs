using Application.Expenses.Commands;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Expenses.Commands.CreatePaymentMethod;

[TestSubject(typeof(ValidatorCreatePaymentMethod))]
public class ValidatorCreatePaymentMethodTest : ValidatorTestBase<ValidatorCreatePaymentMethod, CommandCreatePaymentMethod>
{
    [Fact]
    public void Validate_AllValidData_NoError() =>
        AssertSuccess(new CommandCreatePaymentMethod
        {
            Name = "Test",
            Limit = 1000
        });

    [Fact]
    public void Validate_MinimalValidData_NoError() =>
        AssertSuccess(new CommandCreatePaymentMethod
        {
            Name = "Test",
        });
    
    [Fact]
    public void Validate_EmptyName_Error()
    {
        AssertFailure(new CommandCreatePaymentMethod
        {
            Name = "",
            Limit = 1000
        });
    }
}