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

public abstract class TestHandlerGetById<THandler, TEntity, TDTO, TQuery, TRepository>
    : TestCQRSHandlerBase<THandler, TEntity>
    where THandler : HandlerCRUDGetById<TEntity, TQuery, TDTO>
    where TEntity : class, IEntity, IAutorizable
    where TDTO : class
    where TQuery : QueryCRUDGetById<TDTO>
    where TRepository : class, IRepositoryCRUD<TEntity>
{
    private TEntity _entity;
    private TDTO _dto;

    protected override void InitializeFields()
    {
        base.InitializeFields();
        TestFactoryDTOBase<TEntity, TDTO> factoryDTO = TestFactoriesProvider.GetFactory<TestFactoryDTOBase<TEntity, TDTO>>();
        _entity = FactoryEntity.CreateEntity(FactoryUsers.DefaultUser1Authenticated, GetQuery().Id);
        _dto = factoryDTO.CreateDTO(_entity);
    }

    protected override void CleanUp()
    {
        base.CleanUp();
        _entity = null;
        _dto = null;
    }

    protected abstract TQuery GetQuery();

    protected override void SetUpMocks()
    {
        RegisterMock<TRepository>();

        Mock<IMapper> mockMapper = new();
        mockMapper
            .Setup(m => m.Map<TDTO>(It.Is<TEntity>(entity => entity == _entity)))
            .Returns(_dto);
        RegisterMock<IMapper, Mock<IMapper>>(mockMapper);
        base.SetUpMocks();
    }

    protected async Task GetOne_ValidData_ReturnedOne_Base()
    {
        // Arrange
        TQuery query = GetQuery();
        GetMock<TRepository>()
            .Setup(m => m.GetByIdAsync(It.Is<int>(
                id => id == _entity.Id
            )))
            .ReturnsAsync(_entity);

        // Act
        Result<TDTO> result = await Handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeNullOrEmpty();
        result.Value.Should().NotBeNull();

        TDTO dto = result.Value;
        dto.Should().BeEquivalentTo(_dto);
    }

    protected async Task GetOne_NoEntryAtPassedID_ReturnedNull_Base()
    {
        // Arrange
        TQuery query = GetQuery();

        GetMock<TRepository>()
            .Setup(m => m.GetByIdAsync(It.Is<int>(
                id => id == query.Id
            )))
            .ReturnsAsync((TEntity)null);

        // Act
        Result<TDTO> result = await Handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().NotBeNullOrEmpty();
        result.Value.Should().BeNull();
    }
}