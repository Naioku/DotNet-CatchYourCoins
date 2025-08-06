using Application.Dashboard.Commands;
using Application.Tests.Factories;
using Application.Tests.Factories.DTOs;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Dashboard.Commands;

[TestSubject(typeof(ValidatorCRUDUpdate<,>))]
public class TestValidatorCRUDUpdate
    : TestValidatorBase<
        ValidatorCRUDUpdate<TestDTO, TestValidator<TestDTO>>,
        CommandCRUDUpdate<TestDTO>
    >
{
    [Fact]
    protected void Validate_AllValidData_NoError()
    {
        AssertSuccess(new CommandCRUDUpdate<TestDTO>
        {
            Id = 1,
            Data = TestFactoriesProvider.GetFactory<TestFactoryDTO>().CreateDTO(),
        });
    }

    [Fact]
    protected void Validate_InvalidId_Error()
    {
        AssertFailure(new CommandCRUDUpdate<TestDTO>
        {
            Id = -1,
            Data = TestFactoriesProvider.GetFactory<TestFactoryDTO>().CreateDTO(),
        });
    }
}