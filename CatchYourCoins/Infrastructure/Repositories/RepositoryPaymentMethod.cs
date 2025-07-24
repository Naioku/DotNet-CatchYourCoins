using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Infrastructure.Extensions;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class RepositoryPaymentMethod(
    AppDbContext dbContext,
    IServiceCurrentUser serviceCurrentUser) : IRepositoryPaymentMethod
{
    public async Task CreatePaymentMethodAsync(PaymentMethod paymentMethod) => await dbContext.PaymentMethods.AddAsync(paymentMethod);
    public Task<PaymentMethod?> GetPaymentMethodByIdAsync(int id) =>
        dbContext.PaymentMethods
            .WhereAuthorized(serviceCurrentUser.User.Id)
            .FirstOrDefaultAsync(pm => pm.Id == id);

    public void DeletePaymentMethod(PaymentMethod paymentMethod) => dbContext.PaymentMethods.Remove(paymentMethod);
}