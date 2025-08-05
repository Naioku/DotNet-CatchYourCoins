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

using Query = QueryCRUDGetById<TestDTO>;
using IRepository = IRepositoryCRUD<TestEntity>;

[TestSubject(typeof(HandlerCRUDGetById<,>))]
public abstract class TestHandlerGetById : TestCQRSHandlerBase<HandlerCRUDGetById<TestEntity, TestDTO>, TestEntity>
{
    private TestEntity _entity;
    private TestDTO _dto;

    protected override void InitializeFields()
    {
        base.InitializeFields();
        TestFactoryDTO factoryDTO = TestFactoriesProvider.GetFactory<TestFactoryDTO>();
        _entity = FactoryEntity.CreateEntity(FactoryUsers.DefaultUser1Authenticated, GetQuery().Id);
        _dto = factoryDTO.CreateDTO(_entity);
    }

    protected override void CleanUp()
    {
        base.CleanUp();
        _entity = null;
        _dto = null;
    }

    protected abstract Query GetQuery();

    protected override void SetUpMocks()
    {
        RegisterMock<IRepository>();

        Mock<IMapper> mockMapper = new();
        mockMapper
            .Setup(m => m.Map<TestDTO>(It.Is<TestEntity>(entity => entity == _entity)))
            .Returns(_dto);
        RegisterMock<IMapper, Mock<IMapper>>(mockMapper);
        base.SetUpMocks();
    }

    [Fact]
    private async Task GetOne_ValidData_ReturnedOne_Base()
    {
        // Arrange
        Query query = GetQuery();
        GetMock<IRepository>()
            .Setup(m => m.GetByIdAsync(It.Is<int>(
                id => id == _entity.Id
            )))
            .ReturnsAsync(_entity);

        // Act
        Result<TestDTO> result = await Handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeNullOrEmpty();
        result.Value.Should().NotBeNull();

        TestDTO dto = result.Value;
        dto.Should().BeEquivalentTo(_dto);
    }

    [Fact]
    private async Task GetOne_NoEntryAtPassedID_ReturnedNull_Base()
    {
        // Arrange
        Query query = GetQuery();

        GetMock<IRepository>()
            .Setup(m => m.GetByIdAsync(It.Is<int>(
                id => id == query.Id
            )))
            .ReturnsAsync((TestEntity)null);

        // Act
        Result<TestDTO> result = await Handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().NotBeNullOrEmpty();
        result.Value.Should().BeNull();
    }
}