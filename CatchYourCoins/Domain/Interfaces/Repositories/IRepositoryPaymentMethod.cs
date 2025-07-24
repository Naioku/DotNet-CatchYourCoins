using Domain.Dashboard.Entities;

namespace Domain.Interfaces.Repositories;

public interface IRepositoryPaymentMethod
{
    Task CreatePaymentMethodAsync(PaymentMethod paymentMethod);
    Task<PaymentMethod?> GetPaymentMethodByIdAsync(int id);
    void DeletePaymentMethod(PaymentMethod paymentMethod);
}