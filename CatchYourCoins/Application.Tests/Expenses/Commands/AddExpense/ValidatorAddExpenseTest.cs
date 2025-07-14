using System;
using Application.Expenses.Commands;
using FluentValidation.TestHelper;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Expenses.Commands.AddExpense;

[TestSubject(typeof(ValidatorAddExpense))]
public class ValidatorAddExpenseTest
{
    [Fact]
    public void Validate_AllValidData_NoError()
    {
        // Arrange
        var validator = new ValidatorAddExpense();
        var command = new CommandAddExpense
        {
            Amount = 100,
            Date = DateTime.Now,
            Description = "Test",
            CategoryId = 1,
            PaymentMethodId = 1,
        };
        
        // Act
        var result = validator.TestValidate(command);
        
        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
    
    [Fact]
    public void Validate_MinimumValidData_NoError()
    {
        // Arrange
        var validator = new ValidatorAddExpense();
        var command = new CommandAddExpense
        {
            Amount = 100,
            Date = DateTime.Now,
        };
        
        // Act
        var result = validator.TestValidate(command);
        
        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
    
    [Fact]
    public void Validate_InvalidAmount_Error()
    {
        // Arrange
        var validator = new ValidatorAddExpense();
        var command = new CommandAddExpense
        {
            Amount = -100,
            Date = DateTime.Now,
        };
        
        // Act
        var result = validator.TestValidate(command);
        
        // Assert
        result.ShouldHaveValidationErrors();
    }
}