using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Dashboard.Commands;
using Application.Tests.Factories;
using Application.Tests.Factories.DTOs;
using Application.Tests.TestObjects;
using Application.Tests.TestObjects.Entity;
using AutoMapper;
using Domain;
using Domain.Dashboard.Specifications;
using Domain.Interfaces.Repositories;
using FluentAssertions;
using JetBrains.Annotations;
using Moq;
using Xunit;

namespace Application.Tests.Dashboard.Commands;

using Command = CommandCRUDUpdate<TestObjEntity, TestObjDTO>;
using IRepository = IRepositoryCRUD<TestObjEntity>;

[TestSubject(typeof(HandlerCRUDUpdate<,>))]
public class TestHandlerCRUDUpdate : TestCQRSHandlerBase<HandlerCRUDUpdate<TestObjEntity, TestObjDTO>, TestObjEntity>
{
    private List<TestObjEntity> _entitiesOld;
    private List<TestObjEntity> _entitiesNew;
    private List<TestObjDTO> _dtosNew;

    protected override void InitializeFields()
    {
        base.InitializeFields();
        TestFactoryDTO factoryDTO = TestFactoriesProvider.GetFactory<TestFactoryDTO>();
        _entitiesOld = FactoryEntity.CreateEntities(FactoryUsers.DefaultUser1Authenticated, 2, namePrefix: "TestOld");
        _entitiesNew = FactoryEntity.CreateEntities(FactoryUsers.DefaultUser1Authenticated, 2, namePrefix: "TestNew");
        _dtosNew = factoryDTO.CreateDTOs(_entitiesNew);
    }

    protected override void CleanUp()
    {
        base.CleanUp();
        _entitiesOld = null;
        _entitiesNew = null;
        _dtosNew = null;
    }

    protected override HandlerCRUDUpdate<TestObjEntity, TestObjDTO> CreateHandler() =>
        new(
            GetMock<IRepository>().Object,
            GetMock<IUnitOfWork>().Object,
            GetMock<IMapper>().Object
        );

    protected override void SetUpMocks()
    {
        RegisterMock<IRepository>();
        RegisterMock<IUnitOfWork>();
        RegisterMock<ISpecificationDashboardEntity<TestObjEntity>>();

        Mock<IMapper> mockMapper = new();
        mockMapper
            .Setup(m => m.Map(
                It.Is<IReadOnlyList<TestObjDTO>>(dto => dto == _dtosNew),
                It.Is<IReadOnlyList<TestObjEntity>>(entity => entity == _entitiesOld)
            ))
            .Returns(_entitiesNew);
        RegisterMock<IMapper, Mock<IMapper>>(mockMapper);
        base.SetUpMocks();
    }

    // Todo: Delete "Base" from every test method.
    [Fact]
    private async Task Update_ValidData_EntityUpdated_Base()
    {
        ISpecificationDashboardEntity<TestObjEntity> mockSpecification = GetMock<ISpecificationDashboardEntity<TestObjEntity>>().Object;

        GetMock<IRepository>()
            .Setup(m => m.GetAsync(It.Is<ISpecificationDashboardEntity<TestObjEntity>>(s => s == mockSpecification)))
            .ReturnsAsync(_entitiesOld);

        Command command = new()
        {
            Specification = mockSpecification,
            Data = _dtosNew
        };

        // Act
        Result result = await Handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeNullOrEmpty();

        GetMock<IRepository>().Verify(
            m => m.Update(It.Is<IReadOnlyList<TestObjEntity>>(entities => entities == _entitiesNew)),
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
        ISpecificationDashboardEntity<TestObjEntity> mockSpecification = GetMock<ISpecificationDashboardEntity<TestObjEntity>>().Object;

        GetMock<IRepository>()
            .Setup(m => m.GetAsync(It.Is<ISpecificationDashboardEntity<TestObjEntity>>(s => s == mockSpecification)))
            .ReturnsAsync([]);

        Command command = new()
        {
            Specification = mockSpecification,
            Data = _dtosNew
        };

        // Act
        Result result = await Handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().NotBeNullOrEmpty();

        GetMock<IRepository>().Verify(
            m => m.Update(It.IsAny<IReadOnlyList<TestObjEntity>>()),
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
            .Setup(m => m.Update(It.IsAny<IReadOnlyList<TestObjEntity>>()))
            .Throws(new Exception());

        Command command = new()
        {
            Specification = GetMock<ISpecificationDashboardEntity<TestObjEntity>>().Object,
            Data = _dtosNew
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
            Specification = GetMock<ISpecificationDashboardEntity<TestObjEntity>>().Object,
            Data = _dtosNew
        };

        // Act
        Result result = await Handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().NotBeNullOrEmpty();
    }
}