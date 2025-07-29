using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories;

public class RepositoryPaymentMethod(
    AppDbContext dbContext,
    IServiceCurrentUser serviceCurrentUser)
    : RepositoryCRUD<PaymentMethod>(dbContext, serviceCurrentUser), IRepositoryPaymentMethod;