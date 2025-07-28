using Application.DTOs;
using Application.Requests.Queries.GetById;
using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;

namespace Application.Expenses.Queries.GetById;

public class QueryGetCategoryById : QueryGetByIdBase<CategoryDTO>;

public class HandlerGetCategoryById(IRepositoryCategoryExpenses repository)
    : HandlerCRUDGetById<CategoryExpenses, QueryGetCategoryById, CategoryDTO>(repository)
{
    protected override Dictionary<string, string> GetFailureMessages() =>
        new()
        {
            { "Category", "Category not found" }
        };

    protected override CategoryDTO MapEntityToDTO(CategoryExpenses entity) =>
        new()
        {
            Id = entity.Id,
            Name = entity.Name,
            Limit = entity.Limit,
        };
}