using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Dashboard.Commands;
using Application.Tests.TestObjects.Entity;
using Domain;
using Domain.Dashboard.Specifications;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;
using Moq;
using Xunit;

namespace Application.Tests.Dashboard.Commands;

using Command = CommandCRUDDelete<TestObjEntity>;
using IRepository = IRepositoryCRUD<TestObjEntity>;

[TestSubject(typeof(HandlerCRUDDelete<>))]
public abstract class TestHandlerCRUDDelete : TestCQRSHandlerBase<HandlerCRUDDelete<TestObjEntity>, TestObjEntity>
{
    protected override void SetUpMocks()
    {
        RegisterMock<IRepository>();
        RegisterMock<IUnitOfWork>();
        base.SetUpMocks();
    }

    [Fact]
    private async Task DeleteOne_ValidData_DeletedEntity_Base()
    {
        // Arrange
        ISpecificationDashboardEntity<TestObjEntity> mockSpecification = GetMock<ISpecificationDashboardEntity<TestObjEntity>>().Object;

        TestObjEntity entity = FactoryEntity.CreateEntity(FactoryUsers.DefaultUser1Authenticated);
        GetMock<IRepository>()
            .Setup(m => m.GetAsync(It.Is<ISpecificationDashboardEntity<TestObjEntity>>(s => s == mockSpecification)))
            .ReturnsAsync([entity]);

        Command command = new() { Specification = mockSpecification };

        // Act
        Result result = await Handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Errors);
        GetMock<IRepository>().Verify(
            m => m.Delete(It.Is<IReadOnlyList<TestObjEntity>>(entities => entities[0] == entity)),
            Times.Once
        );
        GetMock<IUnitOfWork>().Verify(m => m.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [Fact]
    private async Task DeleteOne_NoEntryAtPassedID_DeletedNothing_Base()
    {
        // Arrange
        ISpecificationDashboardEntity<TestObjEntity> mockSpecification = GetMock<ISpecificationDashboardEntity<TestObjEntity>>().Object;

        GetMock<IRepository>()
            .Setup(m => m.GetAsync(It.Is<ISpecificationDashboardEntity<TestObjEntity>>(
                s => s == mockSpecification
            )))
            .ReturnsAsync([]);

        Command command = new() { Specification = mockSpecification };

        // Act
        Result result = await Handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotEmpty(result.Errors);
        GetMock<IRepository>().Verify(m => m.Delete(It.IsAny<IReadOnlyList<TestObjEntity>>()), Times.Never);
    }
}