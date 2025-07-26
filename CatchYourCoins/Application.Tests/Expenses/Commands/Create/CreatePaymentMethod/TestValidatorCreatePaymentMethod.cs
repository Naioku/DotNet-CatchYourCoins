using Application.Expenses.Commands.Create;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Expenses.Commands.Create.CreatePaymentMethod;

[TestSubject(typeof(Application.Expenses.Commands.Create.TestValidatorCreatePaymentMethod))]
public class TestValidatorCreatePaymentMethod : TestValidatorBase<Application.Expenses.Commands.Create.TestValidatorCreatePaymentMethod, CommandCreatePaymentMethod>
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