using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Infrastructure.Extensions;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class RepositoryExpense(AppDbContext dbContext, IServiceCurrentUser serviceCurrentUser) : IRepositoryExpense
{
    public async Task CreateAsync(Expense expense) => await dbContext.Expenses.AddAsync(expense);

    public async Task<Expense?> GetByIdAsync(int id) =>
        await dbContext.Expenses
            .WhereAuthorized(serviceCurrentUser.User.Id)
            .FirstOrDefaultAsync(e => e.Id == id);

    public void Delete(Expense expense) => dbContext.Expenses.Remove(expense);
}