using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Application.Requests.Commands;
using Application.Tests.Factories;
using Domain;
using Domain.Interfaces.Repositories;
using Moq;

namespace Application.Tests;

public abstract class TestHandlerCreateRange<THandler, TEntity, TDTO, TCommand, TRepository, TFactory>
    : TestCQRSHandlerBase<THandler, TFactory, TEntity>
    where THandler : HandlerCRUDCreateRange<TEntity, TCommand, TDTO>
    where TEntity : class, IEntity
    where TCommand : CommandCRUDCreateRange<TDTO>
    where TRepository : class, IRepositoryCRUD<TEntity>
    where TFactory : TestFactoryEntityBase<TEntity>, new()
{
    protected abstract TCommand GetCommand();
    protected abstract Expression<Func<IList<TEntity>, bool>> GetRepositoryMatch(TCommand command);

    protected override void SetUpMocks()
    {
        RegisterMock<TRepository>();
        RegisterMock<IUnitOfWork>();
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
            m => m.CreateRangeAsync(It.Is(GetRepositoryMatch(command))),
            Times.Once
        );
        GetMock<IUnitOfWork>().Verify(
            m => m.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once
        );
    }
}