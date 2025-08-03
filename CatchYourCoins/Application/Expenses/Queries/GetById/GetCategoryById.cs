using Application.DTOs.OutputDTOs.Expenses;
using Application.Requests.Queries;
using AutoMapper;
using Domain.Dashboard.Entities.Expenses;
using Domain.Interfaces.Repositories;

namespace Application.Expenses.Queries.GetById;

public class QueryGetCategoryById : QueryCRUDGetById<OutputDTOExpenseCategory>;

public class HandlerGetCategoryById(
    IRepositoryExpenseCategory repository,
    IMapper mapper)
    : HandlerCRUDGetById<
        ExpenseCategory,
        QueryGetCategoryById,
        OutputDTOExpenseCategory
    >(repository, mapper)
{
    protected override Dictionary<string, string> GetFailureMessages() =>
        new()
        {
            { "Category", "Category not found" }
        };
}