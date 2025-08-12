using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Dashboard.Queries;
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

namespace Application.Tests.Dashboard.Queries;

using Query = QueryCRUDGet<TestObjEntity, TestObjDTO>;
using IRepository = IRepositoryCRUD<TestObjEntity>;

[TestSubject(typeof(HandlerCRUDGet<,>))]
public class TestHandlerCRUDGet : TestCQRSHandlerBase<HandlerCRUDGet<TestObjEntity, TestObjDTO>, TestObjEntity>
{
    private List<TestObjEntity> _entities;
    private List<TestObjDTO> _dtos;

    protected override void InitializeFields()
    {
        base.InitializeFields();
        TestFactoryDTO factoryDTO = TestFactoriesProvider.GetFactory<TestFactoryDTO>();
        _entities = FactoryEntity.CreateEntities(FactoryUsers.DefaultUser1Authenticated, 2);
        _dtos = factoryDTO.CreateDTOs(_entities);
    }

    protected override void CleanUp()
    {
        base.CleanUp();
        _entities = null;
        _dtos = null;
    }

    protected override HandlerCRUDGet<TestObjEntity, TestObjDTO> CreateHandler() =>
        new(
            GetMock<IRepositoryCRUD<TestObjEntity>>().Object,
            GetMock<IMapper>().Object
        );

    protected override void SetUpMocks()
    {
        RegisterMock<IRepository>();
        RegisterMock<ISpecificationDashboardEntity<TestObjEntity>>();

        Mock<IMapper> mockMapper = new();
        mockMapper
            .Setup(m => m.Map<IReadOnlyList<TestObjDTO>>(It.Is<IReadOnlyList<TestObjEntity>>(entities => entities == _entities)))
            .Returns(_dtos);
        RegisterMock<IMapper, Mock<IMapper>>(mockMapper);
        base.SetUpMocks();
    }

    [Fact]
    private async Task GetOne_ValidData_ReturnedOne_Base()
    {
        // Arrange
        ISpecificationDashboardEntity<TestObjEntity> mockSpecification = GetMock<ISpecificationDashboardEntity<TestObjEntity>>().Object;
        
        Query query = new()
        {
            Specification = mockSpecification,
        };
        
        GetMock<IRepository>()
            .Setup(m => m.GetAsync(It.Is<ISpecificationDashboardEntity<TestObjEntity>>(
                s => s == mockSpecification
            )))
            .ReturnsAsync(_entities);

        // Act
        Result<IReadOnlyList<TestObjDTO>> result = await Handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeNullOrEmpty();
        result.Value.Should().NotBeNull();

        IReadOnlyList<TestObjDTO> dto = result.Value;
        dto.Should().BeEquivalentTo(_dtos);
    }

    [Fact]
    private async Task GetOne_NoEntryAtPassedID_ReturnedNull_Base()
    {
        // Arrange
        ISpecificationDashboardEntity<TestObjEntity> mockSpecification = GetMock<ISpecificationDashboardEntity<TestObjEntity>>().Object;
        
        Query query = new()
        {
            Specification = mockSpecification,
        };

        GetMock<IRepository>()
            .Setup(m => m.GetAsync(It.Is<ISpecificationDashboardEntity<TestObjEntity>>(
                s => s == mockSpecification
            )))
            .ReturnsAsync([]);

        // Act
        Result<IReadOnlyList<TestObjDTO>> result = await Handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().NotBeNullOrEmpty();
        result.Value.Should().BeNull();
    }
}