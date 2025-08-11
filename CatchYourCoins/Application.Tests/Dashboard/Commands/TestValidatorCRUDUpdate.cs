using Application.Dashboard.Commands;
using Application.Tests.Factories;
using Application.Tests.Factories.DTOs;
using Application.Tests.Factories.Entity;
using Domain.Dashboard.Specifications;
using JetBrains.Annotations;
using Moq;
using Xunit;

namespace Application.Tests.Dashboard.Commands;

[TestSubject(typeof(ValidatorCRUDUpdate<,,>))]
public class TestValidatorCRUDUpdate
    : TestValidatorBase<
        ValidatorCRUDUpdate<TestEntity, TestDTO, TestValidator<TestDTO>>,
        CommandCRUDUpdate<TestEntity, TestDTO>
    >
{
    [Fact]
    protected void Validate_AllValidData_NoError()
    {
        AssertSuccess(new CommandCRUDUpdate<TestEntity, TestDTO>
        {
            Specification = new Mock<ISpecificationDashboardEntity<TestEntity>>().Object,
            Data = TestFactoriesProvider.GetFactory<TestFactoryDTO>().CreateDTOs(2),
        });
    }

    [Fact]
    protected void Validate_EmptyData_Error()
    {
        AssertFailure(new CommandCRUDUpdate<TestEntity, TestDTO>
        {
            Specification = new Mock<ISpecificationDashboardEntity<TestEntity>>().Object,
            Data = [],
        });
    }
}