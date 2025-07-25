using Application.DTOs.Expenses;
using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;

namespace Application.Expenses.Queries;

public class QueryGetCategoryById : QueryGetByIdBase<CategoryDTO>;

public class HandlerGetCategoryById(IRepositoryCategory repositoryCategory)
    : HandlerCRUDGetById<Category, QueryGetCategoryById, CategoryDTO>(repositoryCategory)
{
    protected override Dictionary<string, string> GetFailureMessages() =>
        new()
        {
            { "Category", "Category not found" }
        };

    protected override CategoryDTO MapEntityToDTO(Category entity)
    {
        return new CategoryDTO
        {
            Id = entity.Id,
            Name = entity.Name,
            Limit = entity.Limit,
        };
    }
}