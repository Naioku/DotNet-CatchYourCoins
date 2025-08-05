using Domain.Dashboard.Entities;

namespace Application.Tests.Factories.Entity;

public class TestFactoryEntity : TestFactoryEntityBase<TestEntity>
{
    public override TestEntity CreateEntity(CurrentUser currentUser, int id = 1) => new()
    {
        Id = id,
        Name = $"Test-{id}",
        UserId = currentUser.Id,
    };
}