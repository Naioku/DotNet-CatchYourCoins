using System;
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

public abstract class TestHandlerCreate<THandler, TEntity, TDTO, TCommand, TRepository>
    : TestCQRSHandlerBase<THandler, TEntity>
    where THandler : HandlerCRUDCreate<TEntity, TCommand, TDTO>
    where TEntity : class, IEntity, IAutorizable
    where TDTO : class
    where TCommand : CommandCRUDCreate<TDTO>
    where TRepository : class, IRepositoryCRUD<TEntity>
{
    private TEntity _entity;
    private TDTO _dto;
    
    protected abstract TCommand GetCommand(TDTO dto);

    protected override void InitializeFields()
    {
        base.InitializeFields();
        TestFactoryDTOBase<TEntity, TDTO> factoryDTO = TestFactoriesProvider.GetFactory<TestFactoryDTOBase<TEntity, TDTO>>();
        _entity = FactoryEntity.CreateEntity(FactoryUsers.DefaultUser1Authenticated);
        _dto = factoryDTO.CreateDTO(_entity);
    }

    protected override void CleanUp()
    {
        base.CleanUp();
        _entity = null;
        _dto = null;
    }

    protected override void SetUpMocks()
    {
        RegisterMock<TRepository>();
        RegisterMock<IUnitOfWork>();

        Mock<IMapper> mockMapper = new();
        mockMapper
            .Setup(m => m.Map<TEntity>(It.Is<TDTO>(dto => dto == _dto)))
            .Returns(_entity);
        RegisterMock<IMapper, Mock<IMapper>>(mockMapper);
        base.SetUpMocks();
    }

    protected async Task Create_ValidData_EntityCreated_Base()
    {
        // Arrange
        TCommand command = GetCommand(_dto);

        // Act
        Result result = await Handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Errors.Should().BeNullOrEmpty();
        
        GetMock<TRepository>().Verify(
            m => m.CreateAsync(It.Is<TEntity>(entity => entity == _entity)),
            Times.Once
        );
        GetMock<IUnitOfWork>().Verify(
            m => m.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once
        );
    }
    
    protected async Task Create_RepositoryThrowsException_EntityNotCreated_Base()
    {
        // Arrange
        GetMock<TRepository>()
            .Setup(m => m.CreateAsync(It.IsAny<TEntity>()))
            .ThrowsAsync(new Exception());
        
        TCommand command = GetCommand(_dto);

        // Act
        Result result = await Handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().NotBeNullOrEmpty();
    }
    
    protected async Task Create_UnitOfWorkThrowsException_EntityNotCreated_Base()
    {
        // Arrange
        GetMock<IUnitOfWork>()
            .Setup(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());
        
        TCommand command = GetCommand(_dto);

        // Act
        Result result = await Handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().NotBeNullOrEmpty();
    }
}