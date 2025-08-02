using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Requests.Queries;
using Application.Tests.Factories;
using AutoMapper;
using Domain;
using Domain.Interfaces.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace Application.Tests;

public abstract class TestHandlerGetAll<THandler, TEntity, TDTO, TQuery, TRepository, TFactory>
    : TestCQRSHandlerBase<THandler, TFactory, TEntity>
    where THandler : HandlerCRUDGetAll<TEntity, TQuery, TDTO>
    where TEntity : IEntity
    where TDTO : class
    where TQuery : QueryCRUDGetAll<TDTO>, new()
    where TRepository : class, IRepositoryCRUD<TEntity>
    where TFactory : TestFactoryEntityBase<TEntity>, new()
{
    private List<TEntity> _entities;
    
    protected abstract IReadOnlyList<TDTO> GetMappedDTOs(List<TEntity> entity);

    protected override void InitializeFields()
    {
        base.InitializeFields();
        _entities = FactoryEntity.CreateEntities(TestFactoryUsers.DefaultUser1Authenticated, 5);
    }

    protected override void SetUpMocks()
    {
        RegisterMock<TRepository>();
        
        Mock<IMapper> mockMapper = new();
        mockMapper
            .Setup(m => m.Map<IReadOnlyList<TDTO>>(It.Is<IReadOnlyList<TEntity>>(entities => entities == _entities)))
            .Returns(GetMappedDTOs(_entities));
        RegisterMock<IMapper, Mock<IMapper>>(mockMapper);
        base.SetUpMocks();
    }
    
    protected async Task GetAll_ValidData_ReturnedAll_Base()
    {
        // Arrange
        GetMock<TRepository>()
            .Setup(m => m.GetAllAsync())
            .ReturnsAsync(_entities);
        
        TQuery query = new();

        // Act
        Result<IReadOnlyList<TDTO>> result = await Handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeNullOrEmpty();
        result.Value.Should().NotBeNull();

        IReadOnlyList<TDTO> dtos = result.Value;

        dtos.Should().HaveCount(_entities.Count);
        dtos.Should().BeEquivalentTo(GetMappedDTOs(_entities));
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
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().NotBeNullOrEmpty();
        result.Value.Should().BeNull();
    }
}