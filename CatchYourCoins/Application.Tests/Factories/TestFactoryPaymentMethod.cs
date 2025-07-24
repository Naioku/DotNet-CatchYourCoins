using Domain.Dashboard.Entities;

namespace Application.Tests.Factories;

public class TestFactoryPaymentMethod
{
    public static PaymentMethod CreatePaymentMethod(CurrentUser currentUser) => new()
    {
        Id = 1,
        Limit = 100,
        Name = "Test",
        UserId = currentUser.Id
    };
}