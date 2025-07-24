using Application.Expenses.Commands;
using FluentValidation.TestHelper;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Expenses.Commands.DeleteCategory;

[TestSubject(typeof(ValidatorDeleteCategory))]
public class ValidatorDeleteCategoryTest
{
    [Fact]
    public void DeleteExpense_AllValidData_NoError()
    {
        // Arrange
        ValidatorDeleteCategory validator = new();
        CommandDeleteCategory command = new() { Id = 1 };
        
        // Act
        TestValidationResult<CommandDeleteCategory> result = validator.TestValidate(command);
        
        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
    
    [Fact]
    public void DeleteExpense_InvalidId_Error()
    {
        // Arrange
        ValidatorDeleteCategory validator = new();
        CommandDeleteCategory command = new() { Id = -1 };
        
        // Act
        TestValidationResult<CommandDeleteCategory> result = validator.TestValidate(command);
        
        // Assert
        result.ShouldHaveValidationErrors();
    }
}