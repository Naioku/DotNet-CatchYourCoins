using Application.DTOs.Expenses;
using Application.Requests.Queries.GetById;
using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;

namespace Application.Expenses.Queries.GetById;

public class QueryGetExpenseById : QueryGetByIdBase<ExpenseDTO>;

public class HandlerGetExpenseById(IRepositoryExpense repositoryExpense)
    : HandlerCRUDGetById<Expense, QueryGetExpenseById, ExpenseDTO>(repositoryExpense)
{
    protected override Dictionary<string, string> GetFailureMessages() =>
        new()
        {
            { "Expense", "Expense not found" }
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