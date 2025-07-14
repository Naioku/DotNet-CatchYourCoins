using Application.Expenses.Commands;
using FluentValidation.TestHelper;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Expenses.Commands.AddPaymentMethod;

[TestSubject(typeof(ValidatorAddPaymentMethod))]
public class ValidatorAddPaymentMethodTest
{
    [Fact]
    public void Validate_WithAllValidData_ShouldNotHaveValidationError()
    {
        // Arrange
        var validator = new ValidatorAddPaymentMethod();
        var command = new CommandAddPaymentMethod
        {
            Name = "Test",
            Limit = 1000
        };
        
        // Act
        var result = validator.TestValidate(command);
        
        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
    
    [Fact]
    public void Validate_WithoutLimit_ShouldNotHaveValidationError()
    {
        // Arrange
        var validator = new ValidatorAddPaymentMethod();
        var command = new CommandAddPaymentMethod
        {
            Name = "Test"
        };
        
        // Act
        var result = validator.TestValidate(command);
        
        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
    
    [Fact]
    public void Validate_WithEmptyName_ShouldHaveValidationError()
    {
        // Arrange
        var validator = new ValidatorAddPaymentMethod();
        var command = new CommandAddPaymentMethod
        {
            Name = "",
            Limit = 1000
        };
        
        // Act
        var result = validator.TestValidate(command);
        
        // Assert
        result.ShouldHaveValidationErrors();
    }
}