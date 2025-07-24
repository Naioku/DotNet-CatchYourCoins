using Domain.Dashboard.Entities;

namespace Domain.Interfaces.Repositories;

public interface IRepositoryPaymentMethod
{
    Task CreatePaymentMethodAsync(PaymentMethod paymentMethod);
    Task<PaymentMethod?> GetCategoryByIdAsync(int id);
}