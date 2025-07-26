using Domain.Dashboard.Entities;

namespace Application.Tests.Factories;

public class TestFactoryPaymentMethod : TestFactoryEntityBase<PaymentMethod>
{
    public override PaymentMethod CreateEntity(CurrentUser currentUser, int id = 1) => new()
    {
        Id = id,
        Limit = 100,
        Name = "Test",
        UserId = currentUser.Id
    };
}