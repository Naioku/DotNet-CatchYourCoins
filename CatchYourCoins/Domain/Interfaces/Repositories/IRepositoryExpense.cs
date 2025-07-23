using Domain.Dashboard.Entities;

namespace Domain.Interfaces.Repositories;

public interface IRepositoryExpense
{
    Task CreateExpenseAsync(Expense expense);
    Task<Expense?> GetExpenseByIdAsync(int id);
    void DeleteExpense(Expense expense);
}