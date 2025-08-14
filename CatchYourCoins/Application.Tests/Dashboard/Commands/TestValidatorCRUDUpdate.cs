using Application.Dashboard.Commands;
using Application.Tests.Factories;
using Application.Tests.Factories.DTOs;
using Application.Tests.TestObjects;
using Application.Tests.TestObjects.Entity;
using Domain.Dashboard.Specifications;
using JetBrains.Annotations;
using Moq;
using Xunit;

namespace Application.Tests.Dashboard.Commands;

[TestSubject(typeof(ValidatorCRUDUpdate<,,>))]
public class TestValidatorCRUDUpdate
    : TestValidatorBase<
        ValidatorCRUDUpdate<TestObjEntity, TestObjDTO, TestObjValidator<TestObjDTO>>,
        CommandCRUDUpdate<TestObjEntity, TestObjDTO>
    >
{
    [Fact]
    protected void Validate_AllValidData_NoError()
    {
        AssertSuccess(new CommandCRUDUpdate<TestObjEntity, TestObjDTO>
        {
            Specification = new Mock<ISpecificationDashboardEntity<TestObjEntity>>().Object,
            Data = TestFactoriesProvider.GetFactory<TestFactoryDTO>().CreateDTOs(2),
        });
    }

    [Fact]
    protected void Validate_EmptyData_Error()
    {
        AssertFailure(new CommandCRUDUpdate<TestObjEntity, TestObjDTO>
        {
            Specification = new Mock<ISpecificationDashboardEntity<TestObjEntity>>().Object,
            Data = [],
        });
    }
}