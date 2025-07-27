using Domain.Dashboard.Entities;

namespace Application.Tests.Factories;

public class TestFactoryCategoryIncomes : TestFactoryEntityBase<CategoryIncomes>
{
    public override CategoryIncomes CreateEntity(CurrentUser currentUser, int id = 1) => new()
    {
        Id = id,
        Limit = 100,
        Name = "Test",
        UserId = currentUser.Id,
    };
}