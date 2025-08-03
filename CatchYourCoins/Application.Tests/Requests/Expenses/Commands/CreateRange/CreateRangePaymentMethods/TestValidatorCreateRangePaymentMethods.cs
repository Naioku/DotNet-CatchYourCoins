using Application.DTOs.InputDTOs.Expenses;
using Application.Requests.Expenses.Commands.CreateRange;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Requests.Expenses.Commands.CreateRange.CreateRangePaymentMethods;

[TestSubject(typeof(ValidatorCreateRangePaymentMethods))]
public class TestValidatorCreateRangePaymentMethods
    : TestValidatorBase<ValidatorCreateRangePaymentMethods, CommandCreateRangePaymentMethods>
{
    [Fact]
    public void Validate_AllValidData_NoError() =>
        AssertSuccess(new CommandCreateRangePaymentMethods
        {
            Data = [
                new InputDTOExpensePaymentMethod
                {
                    Name = "Test1",
                    Limit = 1000
                },
                new InputDTOExpensePaymentMethod
                {
                    Name = "Test2",
                    Limit = 2000
                },
            ],
            
        });

    [Fact]
    public void Validate_MinimalValidData_NoError() =>
        AssertSuccess(new CommandCreateRangePaymentMethods
        {
            Data = [
                new InputDTOExpensePaymentMethod
                {
                    Name = "Test1",
                },
                new InputDTOExpensePaymentMethod
                {
                    Name = "Test2",
                },
            ],
        });

    [Fact]
    public void Validate_EmptyName_Error() =>
        AssertFailure(new CommandCreateRangePaymentMethods
        {
            Data = [
                new InputDTOExpensePaymentMethod
                {
                    Name = "",
                    Limit = 1000
                },
                new InputDTOExpensePaymentMethod
                {
                    Name = "",
                    Limit = 2000
                },
            ],
        });
}