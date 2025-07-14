using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories;

public class RepositoryPaymentMethod(AppDbContext dbContext) : IRepositoryPaymentMethod
{
    public async Task CreatePaymentMethodAsync(PaymentMethod paymentMethod) => await dbContext.PaymentMethods.AddAsync(paymentMethod);
}