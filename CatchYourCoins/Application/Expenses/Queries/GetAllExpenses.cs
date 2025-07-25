using Application.DTOs.Expenses;
using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;

namespace Application.Expenses.Queries;

public class QueryGetAllExpenses : QueryGetAllBase<ExpenseDTO>;

public class HandlerGetAllExpenses(IRepositoryExpense repository) : HandlerCRUDGetAll<Expense, QueryGetAllExpenses, ExpenseDTO>(repository)
{
    protected override Dictionary<string, string> GetFailureMessages() =>
        new()
        {
            { "Expenses", "Expenses not found" }
        };

    protected override ExpenseDTO MapEntityToDTO(Expense entity) =>
        new()
        {
            Id = entity.Id,
            Amount = entity.Amount,
            Date = entity.Date,
            Description = entity.Description,
            Category = entity.Category?.Name,
            PaymentMethod = entity.PaymentMethod?.Name,
        };
}