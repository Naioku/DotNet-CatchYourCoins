using Application.DTOs;
using Application.Requests.Queries;
using Domain.Dashboard.Entities.Incomes;
using Domain.Interfaces.Repositories;

namespace Application.Incomes.Queries.GetById;

public class QueryGetCategoryById : QueryGetByIdBase<CategoryDTO>;

public class HandlerGetCategoryById(IRepositoryIncomeCategory repository)
    : HandlerCRUDGetById<IncomeCategory, QueryGetCategoryById, CategoryDTO>(repository)
{
    protected override Dictionary<string, string> GetFailureMessages() =>
        new()
        {
            { "Category", "Category not found" }
        };

    protected override CategoryDTO MapEntityToDTO(IncomeCategory entity) =>
        new()
        {
            Id = entity.Id,
            Name = entity.Name,
            Limit = entity.Limit,
        };
}