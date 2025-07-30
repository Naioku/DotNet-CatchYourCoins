using Application.DTOs.OutputDTOs.Expenses;
using Application.Requests.Queries;
using Domain.Dashboard.Entities.Expenses;
using Domain.Interfaces.Repositories;

namespace Application.Expenses.Queries.GetAll;

public class QueryGetAllExpenses : QueryCRUDGetAll<OutputDTOExpense>;

public class HandlerGetAllExpenses(IRepositoryExpense repository)
    : HandlerCRUDGetAll<Expense, QueryGetAllExpenses, OutputDTOExpense>(repository)
{
    protected override Dictionary<string, string> GetFailureMessages() =>
        new()
        {
            { "Expenses", "Expenses not found" }
        };

    protected override OutputDTOExpense MapEntityToDTO(Expense entity) =>
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