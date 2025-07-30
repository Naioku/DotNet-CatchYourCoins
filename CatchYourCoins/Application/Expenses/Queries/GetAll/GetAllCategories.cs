using Application.DTOs.OutputDTOs;
using Application.DTOs.OutputDTOs.Expenses;
using Application.Requests.Queries;
using Domain.Dashboard.Entities.Expenses;
using Domain.Interfaces.Repositories;

namespace Application.Expenses.Queries.GetAll;

public class QueryGetAllCategories : QueryCRUDGetAll<OutputDTOExpenseCategory>;

public class HandlerGetAllCategories(IRepositoryExpenseCategory repository)
    : HandlerCRUDGetAll<ExpenseCategory, QueryGetAllCategories, OutputDTOExpenseCategory>(repository)
{
    protected override Dictionary<string, string> GetFailureMessages() =>
        new()
        {
            { "Categories", "Categories not found" }
        };

    protected override OutputDTOExpenseCategory MapEntityToDTO(ExpenseCategory entity) =>
        new()
        {
            Id = entity.Id,
            Name = entity.Name,
            Limit = entity.Limit,
        };
}