using Application.Tests.Factories;
using Domain;

namespace Application.Tests;

public abstract class TestCQRSHandlerBase<THandler, TFactory, TEntity> : TestBase
    where THandler : class
    where TFactory : TestFactoryEntityBase<TEntity>, new()
    where TEntity : IEntity
{
    protected THandler Handler { get; private set; }
    protected TFactory FactoryEntity { get; } = new();

    protected override void SetUpMocks() { }
    protected override void SetUpTestedObjects() => Handler = CreateHandler();
    protected override void CleanUp() => Handler = null;
    protected abstract THandler CreateHandler();
}