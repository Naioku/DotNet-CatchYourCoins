using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Dashboard.Commands;
using Application.Tests.Factories;
using Application.Tests.Factories.DTOs;
using Application.Tests.TestObjects;
using Application.Tests.TestObjects.Entity;
using AutoMapper;
using Domain;
using Domain.Interfaces.Repositories;
using FluentAssertions;
using JetBrains.Annotations;
using Moq;
using Xunit;

namespace Application.Tests.Dashboard.Commands;

using Command = CommandCRUDCreate<TestObjDTO>;
using IRepository = IRepositoryCRUD<TestObjEntity>;

[TestSubject(typeof(HandlerCRUDCreate<,>))]
public class TestHandlerCRUDCreate : TestCQRSHandlerBase<HandlerCRUDCreate<TestObjEntity, TestObjDTO>, TestObjEntity>
{
    private TestObjEntity _entity;
    private TestObjDTO _dto;

    protected override void InitializeFields()
    {
        base.InitializeFields();
        TestFactoryDTO factoryDTO = TestFactoriesProvider.GetFactory<TestFactoryDTO>();
        _entity = FactoryEntity.CreateEntity(FactoryUsers.DefaultUser1Authenticated);
        _dto = factoryDTO.CreateDTO(_entity);
    }

    protected override void CleanUp()
    {
        base.CleanUp();
        _entity = null;
        _dto = null;
    }

    protected override HandlerCRUDCreate<TestObjEntity, TestObjDTO> CreateHandler() =>
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
            .Setup(m => m.Map<TestObjEntity>(It.Is<TestObjDTO>(dto => dto == _dto)))
            .Returns(_entity);
        RegisterMock<IMapper, Mock<IMapper>>(mockMapper);
        base.SetUpMocks();
    }

    [Fact]
    private async Task Create_ValidData_EntityCreated()
    {
        // Arrange
        Command command = new() { Data = _dto };

        // Act
        Result result = await Handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeNullOrEmpty();

        GetMock<IRepository>().Verify(
            m => m.CreateAsync(It.Is<TestObjEntity>(entity => entity == _entity)),
            Times.Once
        );
        GetMock<IUnitOfWork>().Verify(
            m => m.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [Fact]
    private async Task Create_RepositoryThrowsException_EntityNotCreated()
    {
        // Arrange
        GetMock<IRepository>()
            .Setup(m => m.CreateAsync(It.IsAny<TestObjEntity>()))
            .ThrowsAsync(new Exception());

        Command command = new() { Data = _dto };

        // Act
        Result result = await Handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().NotBeNullOrEmpty();
    }

    [Fact]
    private async Task Create_UnitOfWorkThrowsException_EntityNotCreated()
    {
        // Arrange
        GetMock<IUnitOfWork>()
            .Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());

        Command command = new() { Data = _dto };

        // Act
        Result result = await Handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().NotBeNullOrEmpty();
    }
}