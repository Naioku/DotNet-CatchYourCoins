using Application.DTOs;
using Application.Requests.Queries.GetAll;
using Domain.Dashboard.Entities;
using Domain.Interfaces.Repositories;

namespace Application.Incomes.Queries.GetAll;

public class QueryGetAllCategories : QueryGetAllBase<CategoryDTO>;

public class HandlerGetAllCategories(IRepositoryCategoryIncomes repository)
    : HandlerCRUDGetAll<CategoryIncomes, QueryGetAllCategories, CategoryDTO>(repository)
{
    protected override Dictionary<string, string> GetFailureMessages() =>
        new()
        {
            { "Categories", "Categories not found" }
        };

    protected override CategoryDTO MapEntityToDTO(CategoryIncomes entity) =>
        new()
        {
            Id = entity.Id,
            Name = entity.Name,
            Limit = entity.Limit,
        };
}