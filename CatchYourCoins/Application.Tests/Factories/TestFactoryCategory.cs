using System.Collections.Generic;
using Domain.Dashboard.Entities;

namespace Application.Tests.Factories;

public class TestFactoryCategory
{
    public static Category CreateCategory(CurrentUser currentUser, int id = 1) => new()
    {
        Id = 1,
        Limit = 100,
        Name = "Test",
        UserId = currentUser.Id,
    };

    public static List<Category> CreateCategories(CurrentUser currentUser, int quantity)
    {
        List<Category> result = [];
        for (int i = 0; i < quantity; i++)
        {
            result.Add(CreateCategory(currentUser, i + 1));
        }
        
        return result;
    }
}