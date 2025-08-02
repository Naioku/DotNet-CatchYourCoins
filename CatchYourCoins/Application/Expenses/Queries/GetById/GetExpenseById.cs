using Application.DTOs.OutputDTOs.Expenses;
using Application.Requests.Queries;
using AutoMapper;
using Domain.Dashboard.Entities.Expenses;
using Domain.Interfaces.Repositories;

namespace Application.Expenses.Queries.GetById;

public class QueryGetExpenseById : QueryCRUDGetById<OutputDTOExpense>;

public class HandlerGetExpenseById(
    IRepositoryExpense repository,
    IMapper mapper)
    : HandlerCRUDGetById<
        Expense,
        QueryGetExpenseById,
        OutputDTOExpense
    >(repository, mapper)
{
    protected override Dictionary<string, string> GetFailureMessages() =>
        new()
        {
            { "Expense", "Expense not found" }
        };
}