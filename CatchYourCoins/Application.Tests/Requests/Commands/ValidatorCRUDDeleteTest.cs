using Application.Requests.Commands;
using Application.Tests.Factories.DTOs;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Requests.Commands;

[TestSubject(typeof(ValidatorCRUDDelete<>))]
public class ValidatorCRUDDeleteTest : TestValidatorBase<ValidatorCRUDDelete<TestDTO>, CommandCRUDDelete<TestDTO>>
{
    [Fact]
    protected void Validate_AllValidData_NoError()
    {
        AssertSuccess(new CommandCRUDDelete<TestDTO>
        {
            Id = 1,
        });
    }

    [Fact]
    protected void Validate_InvalidId_Error()
    {
        AssertFailure(new CommandCRUDDelete<TestDTO>
        {
            Id = -1,
        });
    }
}