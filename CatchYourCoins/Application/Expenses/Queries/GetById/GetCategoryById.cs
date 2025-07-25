using Application.DTOs.Expenses;
using Application.Requests.Queries.GetById;
using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;

namespace Application.Expenses.Queries.GetById;

public class QueryGetCategoryById : QueryGetByIdBase<CategoryDTO>;

public class HandlerGetCategoryById(IRepositoryCategory repositoryCategory)
    : HandlerCRUDGetById<Category, QueryGetCategoryById, CategoryDTO>(repositoryCategory)
{
    protected override Dictionary<string, string> GetFailureMessages() =>
        new()
        {
            { "Category", "Category not found" }
        };

    protected override CategoryDTO MapEntityToDTO(Category entity) =>
        new()
        {
            Id = entity.Id,
            Name = entity.Name,
            Limit = entity.Limit,
        };
}