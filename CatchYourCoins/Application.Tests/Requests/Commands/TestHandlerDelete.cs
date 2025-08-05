using System.Threading;
using System.Threading.Tasks;
using Application.Requests.Commands;
using Application.Tests.Factories.Entity;
using Domain;
using Domain.Interfaces.Repositories;
using JetBrains.Annotations;
using Moq;
using Xunit;

namespace Application.Tests.Requests.Commands;

using Command = CommandCRUDDelete<TestEntity>;
using IRepository = IRepositoryCRUD<TestEntity>;

[TestSubject(typeof(HandlerCRUDDelete<>))]
public abstract class TestHandlerDelete: TestCQRSHandlerBase<HandlerCRUDDelete<TestEntity>, TestEntity>
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
        TestEntity entity = FactoryEntity.CreateEntity(FactoryUsers.DefaultUser1Authenticated);
        GetMock<IRepository>()
            .Setup(m => m.GetByIdAsync(It.Is<int>(
                id => id == entity.Id
            )))
            .ReturnsAsync(entity);

        Command command = new() { Id = entity.Id };

        // Act
        Result result = await Handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Errors);
        GetMock<IRepository>().Verify(m => m.Delete(It.Is<TestEntity>(e => e.Id == command.Id)));
        GetMock<IUnitOfWork>().Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact]
    private async Task DeleteOne_NoEntryAtPassedID_DeletedNothing_Base()
    {
        // Arrange
        GetMock<IRepository>()
            .Setup(m => m.GetByIdAsync(It.Is<int>(
                id => id == 1
            )))
            .ReturnsAsync((TestEntity)null);

        Command command = new() { Id = 1 };

        // Act
        Result result = await Handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotEmpty(result.Errors);
        GetMock<IRepository>().Verify(m => m.Delete(It.IsAny<TestEntity>()), Times.Never);
    }
}