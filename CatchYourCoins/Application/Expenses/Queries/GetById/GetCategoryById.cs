using Application.DTOs.Expenses;
using Application.Requests.Queries.GetById;
using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;

namespace Application.Expenses.Queries.GetById;

public class QueryGetCategoryById : QueryGetByIdBase<CategoryDTO>;

public class HandlerGetCategoryById(IRepositoryCategoryExpenses repositoryCategory)
    : HandlerCRUDGetById<CategoryExpenses, QueryGetCategoryById, CategoryDTO>(repositoryCategory)
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