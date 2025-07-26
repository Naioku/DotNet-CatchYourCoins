using Domain.Dashboard.Entities;

namespace Application.Tests.Factories;

public class TestFactoryCategory : TestFactoryEntityBase<Category>
{
    public override Category CreateEntity(CurrentUser currentUser, int id = 1) => new()
    {
        Id = id,
        Limit = 100,
        Name = "Test",
        UserId = currentUser.Id,
    };
}