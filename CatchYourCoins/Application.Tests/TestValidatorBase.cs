using FluentValidation;
using FluentValidation.TestHelper;

namespace Application.Tests.Requests;

public abstract class TestValidatorBase<TValidator, TValidated>
    where TValidator : AbstractValidator<TValidated>, new()
{
    protected void AssertSuccess(TValidated validatedObj)
    {
        // Arrange
        TValidator validator = new();
        
        // Act
        TestValidationResult<TValidated> result = validator.TestValidate(validatedObj);
        
        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    protected void AssertFailure(TValidated validatedObj)
    {
        // Arrange
        TValidator validator = new();
        
        // Act
        TestValidationResult<TValidated> result = validator.TestValidate(validatedObj);
        
        // Assert
        result.ShouldHaveValidationErrors();
    }
}