using Application.DTOs;
using Application.Requests.Queries;
using Domain.Dashboard.Entities.Incomes;
using Domain.Interfaces.Repositories;

namespace Application.Incomes.Queries.GetAll;

public class QueryGetAllCategories : QueryGetAllBase<CategoryDTO>;

public class HandlerGetAllCategories(IRepositoryIncomeCategory repository)
    : HandlerCRUDGetAll<IncomeCategory, QueryGetAllCategories, CategoryDTO>(repository)
{
    protected override Dictionary<string, string> GetFailureMessages() =>
        new()
        {
            { "Categories", "Categories not found" }
        };

    protected override CategoryDTO MapEntityToDTO(IncomeCategory entity) =>
        new()
        {
            Id = entity.Id,
            Name = entity.Name,
            Limit = entity.Limit,
        };
}