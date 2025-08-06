using System;
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

using Command = CommandCRUDUpdate<TestDTO>;
using IRepository = IRepositoryCRUD<TestEntity>;

[TestSubject(typeof(HandlerCRUDUpdate<,>))]
public class TestHandlerCRUDUpdate : TestCQRSHandlerBase<HandlerCRUDUpdate<TestEntity, TestDTO>, TestEntity>
{
    private TestEntity _entityOld;
    private TestEntity _entityNew;
    private TestDTO _dtoNew;

    protected override void InitializeFields()
    {
        base.InitializeFields();
        TestFactoryDTO factoryDTO = TestFactoriesProvider.GetFactory<TestFactoryDTO>();
        _entityOld = FactoryEntity.CreateEntity(FactoryUsers.DefaultUser1Authenticated, namePrefix: "TestOld");
        _entityNew = FactoryEntity.CreateEntity(FactoryUsers.DefaultUser1Authenticated, namePrefix: "TestNew");
        _dtoNew = factoryDTO.CreateDTO(_entityNew);
    }

    protected override void CleanUp()
    {
        base.CleanUp();
        _entityOld = null;
        _entityNew = null;
        _dtoNew = null;
    }

    protected override HandlerCRUDUpdate<TestEntity, TestDTO> CreateHandler() =>
        new(
            GetMock<IRepository>().Object,
            GetMock<IUnitOfWork>().Object,
            GetMock<IMapper>().Object
        );

    protected override void SetUpMocks()
    {
        RegisterMock<IRepository>();
        RegisterMock<IUnitOfWork>();

        Mock<IMapper> mockMapper = new();
        mockMapper
            .Setup(m => m.Map<TestEntity>(It.Is<TestDTO>(dto => dto == _dtoNew)))
            .Returns(_entityNew);
        RegisterMock<IMapper, Mock<IMapper>>(mockMapper);
        base.SetUpMocks();
    }

    // Todo: Delete "Base" from every test method.
    [Fact]
    private async Task Update_ValidData_EntityUpdated_Base()
    {
        // Arrange
        GetMock<IRepository>()
            .Setup(m => m.GetByIdAsync(It.Is<int>(id => id == _entityOld.Id)))
            .ReturnsAsync(_entityOld);
        
        Command command = new()
        {
            Id = _entityOld.Id,
            Data = _dtoNew
        };

        // Act
        Result result = await Handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeNullOrEmpty();

        GetMock<IRepository>().Verify(
            m => m.Update(It.Is<TestEntity>(entity => entity == _entityNew)),
            Times.Once
        );
        GetMock<IUnitOfWork>().Verify(
            m => m.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once
        );
    }
    
    [Fact]
    private async Task Update_NoEntityAtPassedId_EntityUpdated_Base()
    {
        // Arrange
        GetMock<IRepository>()
            .Setup(m => m.GetByIdAsync(It.Is<int>(id => id == _entityOld.Id)))
            .ReturnsAsync((TestEntity)null);
        
        Command command = new()
        {
            Id = _entityOld.Id,
            Data = _dtoNew
        };

        // Act
        Result result = await Handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().NotBeNullOrEmpty();

        GetMock<IRepository>().Verify(
            m => m.Update(It.IsAny<TestEntity>()),
            Times.Never
        );
        GetMock<IUnitOfWork>().Verify(
            m => m.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Never
        );
    }

    [Fact]
    private async Task Update_RepositoryThrowsException_EntityNotUpdated_Base()
    {
        // Arrange
        GetMock<IRepository>()
            .Setup(m => m.Update(It.IsAny<TestEntity>()))
            .Throws(new Exception());

        Command command = new()
        {
            Id = _entityOld.Id,
            Data = _dtoNew
        };

        // Act
        Result result = await Handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().NotBeNullOrEmpty();
        
        GetMock<IUnitOfWork>().Verify(
            m => m.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Never
        );
    }

    [Fact]
    private async Task Update_UnitOfWorkThrowsException_EntityNotUpdated_Base()
    {
        // Arrange
        GetMock<IUnitOfWork>()
            .Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());

        Command command = new()
        {
            Id = _entityOld.Id,
            Data = _dtoNew
        };

        // Act
        Result result = await Handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().NotBeNullOrEmpty();
    }
}