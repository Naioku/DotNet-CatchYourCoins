using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Requests.Commands;
using Application.Tests.Factories;
using AutoMapper;
using Domain;
using Domain.Interfaces.Repositories;
using FluentAssertions;
using Moq;

namespace Application.Tests;

public abstract class TestHandlerCreateRange<THandler, TEntity, TDTO, TCommand, TRepository, TFactory>
    : TestCQRSHandlerBase<THandler, TFactory, TEntity>
    where THandler : HandlerCRUDCreateRange<TEntity, TCommand, TDTO>
    where TEntity : class, IEntity, IAutorizable
    where TCommand : CommandCRUDCreateRange<TDTO>
    where TRepository : class, IRepositoryCRUD<TEntity>
    where TFactory : TestFactoryEntityBase<TEntity>, new()
{
    protected abstract IEnumerable<TDTO> GetInputDTOs();
    protected abstract TCommand GetCommand();
    protected abstract IList<TEntity> GetMappedEntities();

    protected override void SetUpMocks()
    {
        RegisterMock<TRepository>();
        RegisterMock<IUnitOfWork>();
        Mock<IMapper> mockMapper = new();
        mockMapper
            .Setup(m => m.Map<IList<TEntity>>(It.Is<IList<TDTO>>(dto => CheckDTOContent(dto))))
            .Returns(GetMappedEntities());
        RegisterMock<IMapper, Mock<IMapper>>(mockMapper);
        base.SetUpMocks();
    }

    protected async Task Create_ValidData_EntityCreated_Base()
    {
        // Arrange
        TCommand command = GetCommand();
        
        // Act
        await Handler.Handle(command, CancellationToken.None);

        // Assert
        GetMock<TRepository>().Verify(
            m => m.CreateRangeAsync(It.Is<IEnumerable<TEntity>>(entity => CheckEntity(entity))),
            Times.Once
        );
        GetMock<IUnitOfWork>().Verify(
            m => m.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once
        );
    }
    
    private bool CheckEntity(IEnumerable<TEntity> entities) =>
        CheckFluentAssertions(() =>
        {
            IEnumerable<TEntity> entitiesList = entities.ToList();
            entitiesList.Should().HaveSameCount(GetMappedEntities());
            entitiesList.Should().BeEquivalentTo(GetMappedEntities(), options => options.ExcludingMissingMembers());
            entitiesList.Should().AllSatisfy(e => e.UserId.Should().Be(TestFactoryUsers.DefaultUser1Authenticated.Id));
        });

    private bool CheckDTOContent(IList<TDTO> dtos) =>
        CheckFluentAssertions(() =>
        {
            dtos.Should().HaveSameCount(GetMappedEntities());
            dtos.Should().BeEquivalentTo(GetInputDTOs(), options => options.ExcludingMissingMembers());
        });

    private static bool CheckFluentAssertions(Action assertions)
    {
        try
        {
            assertions();
            return true;
        }
        catch
        {
            return false;
        }
    }
}