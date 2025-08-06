using Application.Dashboard.Commands;
using Application.Tests.Factories;
using Application.Tests.Factories.DTOs;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Dashboard.Commands;

[TestSubject(typeof(ValidatorCRUDCreateRange<,>))]
public class TestValidatorCRUDCreateRange
    : TestValidatorBase<
        ValidatorCRUDCreateRange<TestDTO, TestValidator<TestDTO>>,
        CommandCRUDCreateRange<TestDTO>
    >
{
    [Fact]
    protected void Validate_AllValidData_NoError()
    {
        AssertSuccess(new CommandCRUDCreateRange<TestDTO>
        {
            Data = TestFactoriesProvider.GetFactory<TestFactoryDTO>().CreateDTOs(2),
        });
    }

    [Fact]
    protected void Validate_EmptyData_Error()
    {
        AssertFailure(new CommandCRUDCreateRange<TestDTO>
        {
            Data = [],
        });
    }
}