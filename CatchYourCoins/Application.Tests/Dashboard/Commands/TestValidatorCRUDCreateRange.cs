using Application.Dashboard.Commands;
using Application.Tests.Factories;
using Application.Tests.Factories.DTOs;
using Application.Tests.TestObjects;
using JetBrains.Annotations;
using Xunit;

namespace Application.Tests.Dashboard.Commands;

[TestSubject(typeof(ValidatorCRUDCreateRange<,>))]
public class TestValidatorCRUDCreateRange
    : TestValidatorBase<
        ValidatorCRUDCreateRange<TestObjDTO, TestObjValidator<TestObjDTO>>,
        CommandCRUDCreateRange<TestObjDTO>
    >
{
    [Fact]
    protected void Validate_AllValidData_NoError()
    {
        AssertSuccess(new CommandCRUDCreateRange<TestObjDTO>
        {
            Data = TestFactoriesProvider.GetFactory<TestFactoryDTO>().CreateDTOs(2),
        });
    }

    [Fact]
    protected void Validate_EmptyData_Error()
    {
        AssertFailure(new CommandCRUDCreateRange<TestObjDTO>
        {
            Data = [],
        });
    }
}