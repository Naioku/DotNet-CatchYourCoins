using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Requests.Commands;
using Application.Tests.Factories;
using Application.Tests.Factories.DTOs;
using AutoMapper;
using Domain;
using Domain.Interfaces.Repositories;
using FluentAssertions;
using Moq;

namespace Application.Tests;

public abstract class TestHandlerCreateRange<THandler, TEntity, TDTO, TCommand, TRepository>
    : TestCQRSHandlerBase<THandler, TEntity>
    where THandler : HandlerCRUDCreateRange<TEntity, TCommand, TDTO>
    where TEntity : class, IEntity, IAutorizable
    where TCommand : CommandCRUDCreateRange<TDTO>
    where TRepository : class, IRepositoryCRUD<TEntity>
{
    private List<TEntity> _entities;
    private List<TDTO> _dtos;
    protected abstract TCommand GetCommand(List<TDTO> dtos);
    
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
        RegisterMock<IUnitOfWork>();
        Mock<IMapper> mockMapper = new();
        mockMapper
            .Setup(m => m.Map<IList<TEntity>>(It.Is<IList<TDTO>>(dtos => dtos == _dtos)))
            .Returns(_entities);
        RegisterMock<IMapper, Mock<IMapper>>(mockMapper);
        base.SetUpMocks();
    }

    protected async Task Create_ValidData_EntitiesCreated_Base()
    {
        // Arrange
        TCommand command = GetCommand(_dtos);

        // Act
        await Handler.Handle(command, CancellationToken.None);

        // Assert
        GetMock<TRepository>().Verify(
            m => m.CreateRangeAsync(It.Is<IEnumerable<TEntity>>(entities => entities == _entities)),
            Times.Once
        );
        GetMock<IUnitOfWork>().Verify(
            m => m.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once
        );
    }
    
    protected async Task Create_RepositoryThrowsException_EntitiesNotCreated_Base()
    {
        // Arrange
        GetMock<TRepository>()
            .Setup(m => m.CreateRangeAsync(It.IsAny<IList<TEntity>>()))
            .ThrowsAsync(new Exception());
        
        TCommand command = GetCommand(_dtos);

        // Act
        Result result = await Handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().NotBeNullOrEmpty();
    }
    
    protected async Task Create_UnitOfWorkThrowsException_EntitiesNotCreated_Base()
    {
        // Arrange
        GetMock<IUnitOfWork>()
            .Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());
        
        TCommand command = GetCommand(_dtos);

        // Act
        Result result = await Handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().NotBeNullOrEmpty();
    }
}