using Application.Expenses.Commands;
using FluentValidation.TestHelper;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Expenses.Commands.AddCategory;

[TestSubject(typeof(ValidatorAddCategory))]
public class ValidatorAddCategoryTest
{
    [Fact]
    public void Validate_AllValidData_NoError()
    {
        // Arrange
        var validator = new ValidatorAddCategory();
        var command = new CommandAddCategory
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
    public void Validate_MinimalValidData_NoError()
    {
        // Arrange
        var validator = new ValidatorAddCategory();
        var command = new CommandAddCategory
        {
            Name = "Test"
        };
        
        // Act
        var result = validator.TestValidate(command);
        
        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
    
    [Fact]
    public void Validate_EmptyName_Error()
    {
        // Arrange
        var validator = new ValidatorAddCategory();
        var command = new CommandAddCategory
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