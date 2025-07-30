using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Application.Requests.Commands;
using Application.Tests.Factories;
using Domain;
using Domain.Interfaces.Repositories;
using Moq;

namespace Application.Tests;

public abstract class TestHandlerCreate<THandler, TEntity, TDTO, TCommand, TRepository, TFactory>
    : CQRSHandlerTestBase<THandler, TFactory, TEntity>
    where THandler : HandlerCRUDCreate<TEntity, TCommand, TDTO>
    where TEntity : class, IEntity
    where TCommand : CommandCRUDCreate<TDTO>
    where TRepository : class, IRepositoryCRUD<TEntity>
    where TFactory : TestFactoryEntityBase<TEntity>, new()
{
    protected abstract TCommand GetCommand();
    protected abstract Expression<Func<TEntity, bool>> GetRepositoryMatch(TCommand command);

    public override Task InitializeAsync()
    {
        RegisterMock<TRepository>();
        RegisterMock<IUnitOfWork>();
        return base.InitializeAsync();
    }

    protected async Task Create_ValidData_EntityCreated_Base()
    {
        // Arrange
        TCommand command = GetCommand();
        
        // Act
        await Handler.Handle(command, CancellationToken.None);

        // Assert
        GetMock<TRepository>().Verify(
            m => m.CreateAsync(It.Is(GetRepositoryMatch(command))),
            Times.Once
        );
        GetMock<IUnitOfWork>().Verify(
            m => m.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once
        );
    }
}