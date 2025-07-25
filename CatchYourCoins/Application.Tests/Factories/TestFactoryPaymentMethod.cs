using System.Collections.Generic;
using Domain.Dashboard.Entities;

namespace Application.Tests.Factories;

public class TestFactoryPaymentMethod
{
    public static PaymentMethod CreatePaymentMethod(CurrentUser currentUser, int id = 1) => new()
    {
        Id = 1,
        Limit = 100,
        Name = "Test",
        UserId = currentUser.Id
    };

    public static List<PaymentMethod> CreatePaymentMethods(CurrentUser currentUser, int quantity)
    {
        List<PaymentMethod> result = [];
        for (int i = 0; i < quantity; i++)
        {
            result.Add(CreatePaymentMethod(currentUser, i + 1));
        }
        
        return result;
    }
}