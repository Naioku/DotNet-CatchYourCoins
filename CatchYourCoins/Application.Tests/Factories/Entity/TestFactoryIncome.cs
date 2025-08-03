using System;
using Domain.Dashboard.Entities;
using Domain.Dashboard.Entities.Incomes;

namespace Application.Tests.Factories.Entity;

public class TestFactoryIncome : TestFactoryEntityBase<Income>
{
    public override Income CreateEntity(CurrentUser currentUser, int id = 1) => new()
    {
        Id = id,
        Amount = 100,
        Date = DateTime.Now,
        Description = "Test",
        UserId = currentUser.Id,
        CategoryId = 1,
        Category = new IncomeCategory
        {
            Name = "Test",
            UserId = currentUser.Id,
        },
    };
}