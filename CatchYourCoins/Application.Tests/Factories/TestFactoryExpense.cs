using System;
using Domain.Dashboard.Entities;
using Domain.Dashboard.Entities.Expenses;

namespace Application.Tests.Factories;

public class TestFactoryExpense : TestFactoryEntityBase<Expense>
{
    public override Expense CreateEntity(CurrentUser currentUser, int id = 1) => new()
    {
        Id = id,
        Amount = 100,
        Date = DateTime.Now,
        Description = "Test",
        UserId = currentUser.Id,
        CategoryId = 1,
        Category = new ExpenseCategory
        {
            Name = "Test",
            UserId = currentUser.Id,
        },
        PaymentMethodId = 1,
        PaymentMethod = new ExpensePaymentMethod
        {
            Name = "Test",
            UserId = currentUser.Id,
        },
    };
}