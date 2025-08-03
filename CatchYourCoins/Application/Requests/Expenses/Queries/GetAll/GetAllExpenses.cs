using Application.DTOs.OutputDTOs.Expenses;
using Application.Requests.Base.Queries;
using AutoMapper;
using Domain.Dashboard.Entities.Expenses;
using Domain.Interfaces.Repositories;

namespace Application.Requests.Expenses.Queries.GetAll;

public class QueryGetAllExpenses : QueryCRUDGetAll<OutputDTOExpense>;

public class HandlerGetAllExpenses(
    IRepositoryExpense repository,
    IMapper mapper)
    : HandlerCRUDGetAll<
        Expense,
        QueryGetAllExpenses,
        OutputDTOExpense
    >(repository, mapper)
{
    protected override Dictionary<string, string> GetFailureMessages() =>
        new()
        {
            { "Expenses", "Expenses not found" }
        };
}