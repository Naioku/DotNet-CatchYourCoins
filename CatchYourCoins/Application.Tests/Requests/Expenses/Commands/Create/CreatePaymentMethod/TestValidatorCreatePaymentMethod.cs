using Application.DTOs.InputDTOs.Expenses;
using Application.Requests.Expenses.Commands.Create;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Requests.Expenses.Commands.Create.CreatePaymentMethod;

[TestSubject(typeof(ValidatorCreatePaymentMethod))]
public class TestValidatorCreatePaymentMethod : TestValidatorBase<ValidatorCreatePaymentMethod, CommandCreatePaymentMethod>
{
    [Fact]
    public void Validate_AllValidData_NoError() =>
        AssertSuccess(new CommandCreatePaymentMethod
        {
            Data = new InputDTOExpensePaymentMethod
            {
                Name = "Test",
                Limit = 1000
            }
        });

    [Fact]
    public void Validate_MinimalValidData_NoError() =>
        AssertSuccess(new CommandCreatePaymentMethod
        {
            Data = new InputDTOExpensePaymentMethod
            {
                Name = "Test",
            }
        });
    
    [Fact]
    public void Validate_EmptyName_Error()
    {
        AssertFailure(new CommandCreatePaymentMethod
        {
            Data = new InputDTOExpensePaymentMethod
            {
                Name = "",
                Limit = 1000
            }
        });
    }
}