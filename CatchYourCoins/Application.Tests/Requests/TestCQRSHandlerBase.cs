using Application.Tests.Factories;
using Application.Tests.Factories.Entity;
using Domain;

namespace Application.Tests.Requests;

public abstract class TestCQRSHandlerBase<THandler, TEntity> : TestBase
    where THandler : class
    where TEntity : IEntity
{
    protected THandler Handler { get; private set; }
    protected TestFactoryEntity FactoryEntity { get; } = TestFactoriesProvider.GetFactory<TestFactoryEntity>();

    protected override void SetUpMocks() { }
    protected override void SetUpTestedObjects() => Handler = CreateHandler();
    protected override void CleanUp() => Handler = null;
    protected abstract THandler CreateHandler();
}