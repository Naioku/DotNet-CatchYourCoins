using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Requests.Queries.GetAll;
using Application.Tests.Factories;
using Domain;
using Domain.Interfaces.Repositories;
using Moq;
using Xunit;

namespace Application.Tests;

public abstract class TestHandlerGetAll<THandler, TEntity, TDTO, TQuery, TRepository, TFactory>
    : CQRSHandlerTestBase<THandler, TFactory, TEntity>
    where THandler : HandlerCRUDGetAll<TEntity, TQuery, TDTO>
    where TEntity : IEntity
    where TDTO : class
    where TQuery : QueryGetAllBase<TDTO>, new()
    where TRepository : class, IRepositoryCRUD<TEntity>
    where TFactory : TestFactoryEntityBase<TEntity>, new()
{
    public override Task InitializeAsync()
    {
        RegisterMock<TRepository>();
        return base.InitializeAsync();
    }
    
    protected async Task GetAll_ValidData_ReturnedAll_Base(Action<TEntity, TDTO> assertions)
    {
        // Arrange
        List<TEntity> entities = FactoryEntity.CreateEntities(TestFactoryUsers.DefaultUser1Authenticated, 5);
        GetMock<TRepository>()
            .Setup(m => m.GetAllAsync())
            .ReturnsAsync(entities);
        
        TQuery query = new();

        // Act
        Result<IReadOnlyList<TDTO>> result = await Handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Errors);
        Assert.NotNull(result.Value);

        IReadOnlyList<TDTO> dtos = result.Value;

        for (var i = 0; i < dtos.Count; i++)
        {
            assertions(entities[i], dtos[i]);
        }
    }
    
    protected async Task GetAll_NoEntryInDB_ReturnedNull_Base()
    {
        // Arrange
        GetMock<TRepository>()
            .Setup(m => m.GetAllAsync())
            .ReturnsAsync([]);
        
        TQuery query = new();

        // Act
        Result<IReadOnlyList<TDTO>> result = await Handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotEmpty(result.Errors);
        Assert.Null(result.Value);
    }
}