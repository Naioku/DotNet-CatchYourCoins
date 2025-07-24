using Domain.Dashboard.Entities;

namespace Application.Tests.Factories;

public class TestFactoryCategory
{
    public static Category CreateCategory(CurrentUser currentUser) => new()
    {
        Id = 1,
        Limit = 100,
        Name = "Test",
        UserId = currentUser.Id,
    };
}