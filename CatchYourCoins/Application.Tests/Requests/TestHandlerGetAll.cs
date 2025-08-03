using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Requests.Base.Queries;
using Application.Tests.Factories;
using Application.Tests.Factories.DTOs;
using AutoMapper;
using Domain;
using Domain.Interfaces.Repositories;
using FluentAssertions;
using Moq;

namespace Application.Tests.Requests;

public abstract class TestHandlerGetAll<THandler, TEntity, TDTO, TQuery, TRepository>
    : TestCQRSHandlerBase<THandler, TEntity>
    where THandler : HandlerCRUDGetAll<TEntity, TQuery, TDTO>
    where TEntity : IEntity
    where TDTO : class
    where TQuery : QueryCRUDGetAll<TDTO>, new()
    where TRepository : class, IRepositoryCRUD<TEntity>
{
    private List<TEntity> _entities;
    private List<TDTO> _dtos;
    
    protected override void InitializeFields()
    {
        base.InitializeFields();
        TestFactoryDTOBase<TEntity, TDTO> factoryDTO = TestFactoriesProvider.GetFactory<TestFactoryDTOBase<TEntity, TDTO>>();
        _entities = FactoryEntity.CreateEntities(FactoryUsers.DefaultUser1Authenticated, 5);
        _dtos = factoryDTO.CreateDTOs(_entities);
    }
    
    protected override void CleanUp()
    {
        base.CleanUp();
        _entities = null;
        _dtos = null;
    }

    protected override void SetUpMocks()
    {
        RegisterMock<TRepository>();
        
        Mock<IMapper> mockMapper = new();
        mockMapper
            .Setup(m => m.Map<IReadOnlyList<TDTO>>(It.Is<IReadOnlyList<TEntity>>(entities => entities == _entities)))
            .Returns(_dtos);
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
        dtos.Should().BeEquivalentTo(_dtos);
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