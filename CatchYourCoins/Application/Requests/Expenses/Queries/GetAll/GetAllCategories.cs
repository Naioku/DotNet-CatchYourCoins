using Application.DTOs.OutputDTOs.Expenses;
using Application.Requests.Base.Queries;
using AutoMapper;
using Domain.Dashboard.Entities.Expenses;
using Domain.Interfaces.Repositories;

namespace Application.Requests.Expenses.Queries.GetAll;

public class QueryGetAllCategories : QueryCRUDGetAll<OutputDTOExpenseCategory>;

public class HandlerGetAllCategories(
    IRepositoryExpenseCategory repository,
    IMapper mapper)
    : HandlerCRUDGetAll<
        ExpenseCategory,
        QueryGetAllCategories,
        OutputDTOExpenseCategory
    >(repository, mapper)
{
    protected override Dictionary<string, string> GetFailureMessages() =>
        new()
        {
            { "Categories", "Categories not found" }
        };
}