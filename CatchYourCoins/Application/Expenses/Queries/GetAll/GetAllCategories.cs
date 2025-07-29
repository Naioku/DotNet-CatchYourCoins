using Application.DTOs;
using Application.Requests.Queries.GetAll;
using Domain.Dashboard.Entities.Expenses;
using Domain.Interfaces.Repositories;

namespace Application.Expenses.Queries.GetAll;

public class QueryGetAllCategories : QueryGetAllBase<CategoryDTO>;

public class HandlerGetAllCategories(IRepositoryExpenseCategory repository)
    : HandlerCRUDGetAll<ExpenseCategory, QueryGetAllCategories, CategoryDTO>(repository)
{
    protected override Dictionary<string, string> GetFailureMessages() =>
        new()
        {
            { "Categories", "Categories not found" }
        };

    protected override CategoryDTO MapEntityToDTO(ExpenseCategory entity) =>
        new()
        {
            Id = entity.Id,
            Name = entity.Name,
            Limit = entity.Limit,
        };
}