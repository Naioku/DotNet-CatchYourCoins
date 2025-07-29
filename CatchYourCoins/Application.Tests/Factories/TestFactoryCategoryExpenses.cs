using Domain.Dashboard.Entities;
using Domain.Dashboard.Entities.Expenses;

namespace Application.Tests.Factories;

public class TestFactoryCategoryExpenses : TestFactoryEntityBase<ExpenseCategory>
{
    public override ExpenseCategory CreateEntity(CurrentUser currentUser, int id = 1) => new()
    {
        Id = id,
        Limit = 100,
        Name = "Test",
        UserId = currentUser.Id,
    };
}