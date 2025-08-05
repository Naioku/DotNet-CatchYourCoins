using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Dashboard.Commands;
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

namespace Application.Tests.Dashboard.Commands;

using Command = CommandCRUDCreateRange<TestDTO>;
using IRepository = IRepositoryCRUD<TestEntity>;

[TestSubject(typeof(HandlerCRUDCreateRange<,>))]
public abstract class TestHandlerCreateRange : TestCQRSHandlerBase<HandlerCRUDCreateRange<TestEntity, TestDTO>, TestEntity>
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
        RegisterMock<IUnitOfWork>();
        Mock<IMapper> mockMapper = new();
        mockMapper
            .Setup(m => m.Map<IList<TestEntity>>(It.Is<IList<TestDTO>>(dtos => dtos == _dtos)))
            .Returns(_entities);
        RegisterMock<IMapper, Mock<IMapper>>(mockMapper);
        base.SetUpMocks();
    }

    [Fact]
    private async Task Create_ValidData_EntitiesCreated_Base()
    {
        // Arrange
        Command command = new() { Data = _dtos };

        // Act
        await Handler.Handle(command, CancellationToken.None);

        // Assert
        GetMock<IRepository>().Verify(
            m => m.CreateRangeAsync(It.Is<IEnumerable<TestEntity>>(entities => entities == _entities)),
            Times.Once
        );
        GetMock<IUnitOfWork>().Verify(
            m => m.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [Fact]
    private async Task Create_RepositoryThrowsException_EntitiesNotCreated_Base()
    {
        // Arrange
        GetMock<IRepository>()
            .Setup(m => m.CreateRangeAsync(It.IsAny<IList<TestEntity>>()))
            .ThrowsAsync(new Exception());
        
        Command command = new() { Data = _dtos };

        // Act
        Result result = await Handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().NotBeNullOrEmpty();
    }

    [Fact]
    private async Task Create_UnitOfWorkThrowsException_EntitiesNotCreated_Base()
    {
        // Arrange
        GetMock<IUnitOfWork>()
            .Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());
        
        Command command = new() { Data = _dtos };

        // Act
        Result result = await Handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().NotBeNullOrEmpty();
    }
}