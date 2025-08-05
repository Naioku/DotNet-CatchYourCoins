using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Dashboard.Queries;
using Application.Tests.Factories;
using Application.Tests.Factories.DTOs;
using Application.Tests.Factories.Entity;
using AutoMapper;
using Domain;
using Domain.Interfaces.Repositories;
using FluentAssertions;
using JetBrains.Annotations;
using Moq;
using Xunit;

namespace Application.Tests.Dashboard.Queries;

using Query = QueryCRUDGetAll<TestDTO>;
using IRepository = IRepositoryCRUD<TestEntity>;

[TestSubject(typeof(HandlerCRUDGetAll<,>))]
public abstract class TestHandlerGetAll : TestCQRSHandlerBase<HandlerCRUDGetAll<TestEntity, TestDTO>, TestEntity>
{
    private List<TestEntity> _entities;
    private List<TestDTO> _dtos;
    
    protected override void InitializeFields()
    {
        base.InitializeFields();
        TestFactoryDTO factoryDTO = TestFactoriesProvider.GetFactory<TestFactoryDTO>();
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
        RegisterMock<IRepository>();
        
        Mock<IMapper> mockMapper = new();
        mockMapper
            .Setup(m => m.Map<IReadOnlyList<TestDTO>>(It.Is<IReadOnlyList<TestEntity>>(entities => entities == _entities)))
            .Returns(_dtos);
        RegisterMock<IMapper, Mock<IMapper>>(mockMapper);
        base.SetUpMocks();
    }

    [Fact]
    private async Task GetAll_ValidData_ReturnedAll_Base()
    {
        // Arrange
        GetMock<IRepository>()
            .Setup(m => m.GetAllAsync())
            .ReturnsAsync(_entities);
        
        Query query = new();

        // Act
        Result<IReadOnlyList<TestDTO>> result = await Handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeNullOrEmpty();
        result.Value.Should().NotBeNull();

        IReadOnlyList<TestDTO> dtos = result.Value;

        dtos.Should().HaveCount(_entities.Count);
        dtos.Should().BeEquivalentTo(_dtos);
    }
    
    [Fact]
    private async Task GetAll_NoEntryInDB_ReturnedNull_Base()
    {
        // Arrange
        GetMock<IRepository>()
            .Setup(m => m.GetAllAsync())
            .ReturnsAsync([]);
        
        Query query = new();

        // Act
        Result<IReadOnlyList<TestDTO>> result = await Handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().NotBeNullOrEmpty();
        result.Value.Should().BeNull();
    }
}