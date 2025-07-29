using System.Threading;
using System.Threading.Tasks;
using Application.Requests.Commands;
using Application.Tests.Factories;
using Domain;
using Domain.Interfaces.Repositories;
using Moq;
using Xunit;

namespace Application.Tests;

public abstract class TestHandlerDelete<THandler, TEntity, TCommand, TRepository, TFactory, TUnitOfWork>
    : CQRSHandlerTestBase<THandler, TFactory, TEntity>
    where THandler : HandlerCRUDDelete<TEntity, TCommand>
    where TEntity : class, IEntity
    where TCommand : CommandDeleteBase
    where TRepository : class, IRepositoryCRUD<TEntity>
    where TFactory : TestFactoryEntityBase<TEntity>, new()
    where TUnitOfWork : class, IUnitOfWork
{
    public override Task InitializeAsync()
    {
        RegisterMock<TRepository>();
        RegisterMock<IUnitOfWork>();
        return base.InitializeAsync();
    }
    
    protected abstract TCommand GetCommand();
    
    protected async Task DeleteOne_ValidData_DeletedEntity_Base()
    {
        // Arrange
        TEntity entity = FactoryEntity.CreateEntity(TestFactoryUsers.DefaultUser1Authenticated);
        GetMock<TRepository>()
            .Setup(m => m.GetByIdAsync(It.Is<int>(
                id => id == entity.Id
            )))
            .ReturnsAsync(entity);

        TCommand command = GetCommand();

        // Act
        Result result = await Handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Errors);
        GetMock<TRepository>().Verify(m => m.Delete(It.Is<TEntity>(e => e.Id == command.Id)));
        GetMock<TUnitOfWork>().Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
    
    protected async Task DeleteOne_NoEntryAtPassedID_DeletedNothing_Base()
    {
        // Arrange
        GetMock<TRepository>()
            .Setup(m => m.GetByIdAsync(It.Is<int>(
                id => id == 1
            )))
            .ReturnsAsync((TEntity)null);

        TCommand command = GetCommand();

        // Act
        Result result = await Handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotEmpty(result.Errors);
        GetMock<TRepository>().Verify(m => m.Delete(It.IsAny<TEntity>()), Times.Never);
    }
}