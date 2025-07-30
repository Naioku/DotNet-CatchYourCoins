using Application.DTOs;
using Application.DTOs.OutputDTOs;
using Application.DTOs.OutputDTOs.Expenses;
using Application.Requests.Queries;
using Domain.Dashboard.Entities.Expenses;
using Domain.Interfaces.Repositories;

namespace Application.Expenses.Queries.GetById;

public class QueryGetCategoryById : QueryCRUDGetById<OutputDTOExpenseCategory>;

public class HandlerGetCategoryById(IRepositoryExpenseCategory repository)
    : HandlerCRUDGetById<ExpenseCategory, QueryGetCategoryById, OutputDTOExpenseCategory>(repository)
{
    protected override Dictionary<string, string> GetFailureMessages() =>
        new()
        {
            { "Category", "Category not found" }
        };

    protected override OutputDTOExpenseCategory MapEntityToDTO(ExpenseCategory entity) =>
        new()
        {
            Id = entity.Id,
            Name = entity.Name,
            Limit = entity.Limit,
        };
}