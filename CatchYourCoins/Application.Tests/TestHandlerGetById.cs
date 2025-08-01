using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Requests.Queries;
using Application.Tests.Factories;
using Domain;
using Domain.Interfaces.Repositories;
using Moq;
using Xunit;

namespace Application.Tests;

public abstract class TestHandlerGetById<THandler, TEntity, TDTO, TQuery, TRepository, TFactory>
    : TestCQRSHandlerBase<THandler, TFactory, TEntity>
    where THandler : HandlerCRUDGetById<TEntity, TQuery, TDTO>
    where TEntity : class, IEntity
    where TDTO : class
    where TQuery : QueryCRUDGetById<TDTO>
    where TRepository : class, IRepositoryCRUD<TEntity>
    where TFactory : TestFactoryEntityBase<TEntity>, new()
{
    protected override void SetUpMocks()
    {
        RegisterMock<TRepository>();
        base.SetUpMocks();
    }
    
    protected abstract TQuery GetQuery();
    
    protected async Task GetOne_ValidData_ReturnedOne_Base(Action<TEntity, TDTO> assertions)
    {
        // Arrange
        TQuery query = GetQuery();
        TEntity entity = FactoryEntity.CreateEntity(TestFactoryUsers.DefaultUser1Authenticated, query.Id);
        GetMock<TRepository>()
            .Setup(m => m.GetByIdAsync(It.Is<int>(
                id => id == entity.Id
            )))
            .ReturnsAsync(entity);

        // Act
        Result<TDTO> result = await Handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Errors);
        Assert.NotNull(result.Value);

        TDTO dto = result.Value;
        assertions(entity, dto);
    }
    
    protected async Task GetOne_NoEntryAtPassedID_ReturnedNull_Base()
    {
        // Arrange
        TQuery query = GetQuery();

        GetMock<TRepository>()
            .Setup(m => m.GetByIdAsync(It.Is<int>(
                id => id == query.Id
            )))
            .ReturnsAsync((TEntity)null);

        // Act
        Result<TDTO> result = await Handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotEmpty(result.Errors);
        Assert.Null(result.Value);
    }
}