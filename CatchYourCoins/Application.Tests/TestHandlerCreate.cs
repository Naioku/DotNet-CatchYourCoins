using System;
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

public abstract class TestHandlerCreate<THandler, TEntity, TDTO, TCommand, TRepository, TFactory>
    : TestCQRSHandlerBase<THandler, TFactory, TEntity>
    where THandler : HandlerCRUDCreate<TEntity, TCommand, TDTO>
    where TEntity : class, IEntity, IAutorizable
    where TCommand : CommandCRUDCreate<TDTO>
    where TRepository : class, IRepositoryCRUD<TEntity>
    where TFactory : TestFactoryEntityBase<TEntity>, new()
{
    protected abstract TDTO GetInputDTO();
    protected abstract TCommand GetCommand();
    protected abstract TEntity GetMappedEntity();

    protected override void SetUpMocks()
    {
        RegisterMock<TRepository>();
        RegisterMock<IUnitOfWork>();
        
        Mock<IMapper> mockMapper = new();
        mockMapper
            .Setup(m => m.Map<TEntity>(It.Is<TDTO>(dto => CheckDTOContent(dto))))
            .Returns(GetMappedEntity());
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
            m => m.CreateAsync(It.Is<TEntity>(entity => CheckEntity(entity))),
            Times.Once
        );
        GetMock<IUnitOfWork>().Verify(
            m => m.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    private bool CheckEntity(TEntity entity) =>
        CheckFluentAssertions(() =>
        {
            entity.Should().BeEquivalentTo(GetMappedEntity(), options => options.ExcludingMissingMembers());
            entity.UserId.Should().Be(TestFactoryUsers.DefaultUser1Authenticated.Id);
        });

    private bool CheckDTOContent(TDTO dto) => CheckFluentAssertions(() =>
    {
        dto.Should().BeEquivalentTo(GetInputDTO(), options => options.ExcludingMissingMembers());
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