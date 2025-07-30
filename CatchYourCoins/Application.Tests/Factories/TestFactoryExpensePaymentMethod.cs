using Domain.Dashboard.Entities;
using Domain.Dashboard.Entities.Expenses;

namespace Application.Tests.Factories;

public class TestFactoryExpensePaymentMethod : TestFactoryEntityBase<ExpensePaymentMethod>
{
    public override ExpensePaymentMethod CreateEntity(CurrentUser currentUser, int id = 1) => new()
    {
        Id = id,
        Limit = 100,
        Name = "Test",
        UserId = currentUser.Id
    };
}