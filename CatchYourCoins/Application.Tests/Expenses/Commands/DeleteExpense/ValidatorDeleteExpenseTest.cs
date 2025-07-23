using Application.Expenses.Commands;
using FluentValidation.TestHelper;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Expenses.Commands.DeleteExpense;

[TestSubject(typeof(ValidatorDeleteExpense))]
public class ValidatorDeleteExpenseTest
{
    [Fact]
    public void DeleteExpense_AllValidData_NoError()
    {
        // Arrange
        var validator = new ValidatorDeleteExpense();
        var command = new CommandDeleteExpense { Id = 1 };
        
        // Act
        var result = validator.TestValidate(command);
        
        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
    
    [Fact]
    public void DeleteExpense_InvalidId_Error()
    {
        // Arrange
        var validator = new ValidatorDeleteExpense();
        var command = new CommandDeleteExpense { Id = -1 };
        
        // Act
        var result = validator.TestValidate(command);
        
        // Assert
        result.ShouldHaveValidationErrors();
    }
}