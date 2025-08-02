using System;
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

public abstract class TestHandlerGetById<THandler, TEntity, TDTO, TQuery, TRepository, TFactory>
    : TestCQRSHandlerBase<THandler, TFactory, TEntity>
    where THandler : HandlerCRUDGetById<TEntity, TQuery, TDTO>
    where TEntity : class, IEntity, IAutorizable
    where TDTO : class
    where TQuery : QueryCRUDGetById<TDTO>
    where TRepository : class, IRepositoryCRUD<TEntity>
    where TFactory : TestFactoryEntityBase<TEntity>, new()
{
    private TEntity _entity;

    protected override void InitializeFields()
    {
        base.InitializeFields();
        _entity = FactoryEntity.CreateEntity(TestFactoryUsers.DefaultUser1Authenticated, GetQuery().Id);
    }

    protected abstract TQuery GetQuery();
    protected abstract TDTO GetMappedDTO(TEntity entity);

    protected override void SetUpMocks()
    {
        RegisterMock<TRepository>();

        Mock<IMapper> mockMapper = new();
        mockMapper
            .Setup(m => m.Map<TDTO>(It.Is<TEntity>(entity => entity == _entity)))
            .Returns(GetMappedDTO(_entity));
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
        dto.Should().BeEquivalentTo(GetMappedDTO(_entity));
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