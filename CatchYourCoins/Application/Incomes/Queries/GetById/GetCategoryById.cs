using Application.DTOs.OutputDTOs.Incomes;
using Application.Requests.Queries;
using Domain.Dashboard.Entities.Incomes;
using Domain.Interfaces.Repositories;

namespace Application.Incomes.Queries.GetById;

public class QueryGetCategoryById : QueryCRUDGetById<OutputDTOIncomeCategory>;

public class HandlerGetCategoryById(IRepositoryIncomeCategory repository)
    : HandlerCRUDGetById<IncomeCategory, QueryGetCategoryById, OutputDTOIncomeCategory>(repository)
{
    protected override Dictionary<string, string> GetFailureMessages() =>
        new()
        {
            { "Category", "Category not found" }
        };

    protected override OutputDTOIncomeCategory MapEntityToDTO(IncomeCategory entity) =>
        new()
        {
            Id = entity.Id,
            Name = entity.Name,
            Limit = entity.Limit,
        };
}