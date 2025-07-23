using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Infrastructure.Extensions;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class RepositoryExpense(AppDbContext dbContext, IServiceCurrentUser serviceCurrentUser) : IRepositoryExpense
{
    public async Task CreateExpenseAsync(Expense expense) => await dbContext.Expenses.AddAsync(expense);

    public async Task<Expense?> GetExpenseByIdAsync(int id) =>
        await dbContext.Expenses
            .WhereAuthorized(serviceCurrentUser.User.Id)
            .FirstOrDefaultAsync(e => e.Id == id);

    public void DeleteExpense(Expense expense) => dbContext.Expenses.Remove(expense);
}