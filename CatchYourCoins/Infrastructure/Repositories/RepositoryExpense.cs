using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories;

public class RepositoryExpense(AppDbContext dbContext) : IRepositoryExpense
{
    public async Task CreateExpenseAsync(Expense expense) => await dbContext.Expenses.AddAsync(expense);
}