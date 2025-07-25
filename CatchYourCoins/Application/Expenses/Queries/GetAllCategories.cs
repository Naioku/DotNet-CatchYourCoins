using Application.DTOs.Expenses;
using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;

namespace Application.Expenses.Queries;

public class QueryGetAllCategories : QueryGetAllBase<CategoryDTO>;

public class HandlerGetAllCategories(IRepositoryCategory repository) : HandlerCRUDGetAll<Category, QueryGetAllCategories, CategoryDTO>(repository)
{
    protected override Dictionary<string, string> GetFailureMessages() =>
        new()
        {
            { "Categories", "Categories not found" }
        };

    protected override CategoryDTO MapEntityToDTO(Category entity) =>
        new()
        {
            Id = entity.Id,
            Name = entity.Name,
            Limit = entity.Limit,
        };
}