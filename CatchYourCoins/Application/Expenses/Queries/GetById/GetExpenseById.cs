using Application.DTOs.OutputDTOs.Expenses;
using Application.Requests.Queries;
using Domain.Dashboard.Entities.Expenses;
using Domain.Interfaces.Repositories;

namespace Application.Expenses.Queries.GetById;

public class QueryGetExpenseById : QueryCRUDGetById<OutputDTOExpense>;

public class HandlerGetExpenseById(IRepositoryExpense repository)
    : HandlerCRUDGetById<Domain.Dashboard.Entities.Expenses.Expense, QueryGetExpenseById, OutputDTOExpense>(repository)
{
    protected override Dictionary<string, string> GetFailureMessages() =>
        new()
        {
            { "Expense", "Expense not found" }
        };

    protected override OutputDTOExpense MapEntityToDTO(Domain.Dashboard.Entities.Expenses.Expense entity) =>
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