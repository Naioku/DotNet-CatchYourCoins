using FluentValidation;
using FluentValidation.TestHelper;

namespace Application.Tests.Requests;

public abstract class TestValidatorBase<TValidator, TCommand>
    where TValidator : AbstractValidator<TCommand>, new()
{
    protected void AssertSuccess(TCommand command)
    {
        // Arrange
        TValidator validator = new();
        
        // Act
        TestValidationResult<TCommand> result = validator.TestValidate(command);
        
        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    protected void AssertFailure(TCommand command)
    {
        // Arrange
        TValidator validator = new();
        
        // Act
        TestValidationResult<TCommand> result = validator.TestValidate(command);
        
        // Assert
        result.ShouldHaveValidationErrors();
    }
}