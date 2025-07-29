using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories;

public class RepositoryExpense(
    AppDbContext dbContext,
    IServiceCurrentUser serviceCurrentUser)
    : RepositoryCRUD<Expense>(dbContext, serviceCurrentUser), IRepositoryExpense;