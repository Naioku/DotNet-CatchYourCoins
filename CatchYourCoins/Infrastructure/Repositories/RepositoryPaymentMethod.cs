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
    public async Task CreateAsync(PaymentMethod paymentMethod) => await dbContext.PaymentMethods.AddAsync(paymentMethod);

    public Task<PaymentMethod?> GetByIdAsync(int id) =>
        dbContext.PaymentMethods
            .WhereAuthorized(serviceCurrentUser.User.Id)
            .FirstOrDefaultAsync(pm => pm.Id == id);

    public Task<List<PaymentMethod>> GetAllAsync() =>
        dbContext.PaymentMethods
            .WhereAuthorized(serviceCurrentUser.User.Id)
            .ToListAsync();

    public void Delete(PaymentMethod paymentMethod) => dbContext.PaymentMethods.Remove(paymentMethod);
}