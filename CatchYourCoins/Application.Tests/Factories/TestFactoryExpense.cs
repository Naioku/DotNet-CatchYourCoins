using System;
using Domain.Dashboard.Entities;

namespace Application.Tests.Factories;

public static class TestFactoryExpense
{
    public static Expense CreateExpense(CurrentUser currentUser) => new()
    {
        Id = 1,
        Amount = 100,
        Date = DateTime.Now,
        Description = "Test",
        UserId = currentUser.Id,
        CategoryId = 1,
        Category = new Category
        {
            Name = "Test",
            UserId = currentUser.Id,
        },
        PaymentMethodId = 1,
        PaymentMethod = new PaymentMethod
        {
            Name = "Test",
            UserId = currentUser.Id,
        },
    };
}