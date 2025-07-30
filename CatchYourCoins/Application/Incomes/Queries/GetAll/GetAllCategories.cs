using Application.DTOs;
using Application.DTOs.OutputDTOs;
using Application.DTOs.OutputDTOs.Incomes;
using Application.Requests.Queries;
using Domain.Dashboard.Entities.Incomes;
using Domain.Interfaces.Repositories;

namespace Application.Incomes.Queries.GetAll;

public class QueryGetAllCategories : QueryCRUDGetAll<OutputDTOIncomeCategory>;

public class HandlerGetAllCategories(IRepositoryIncomeCategory repository)
    : HandlerCRUDGetAll<IncomeCategory, QueryGetAllCategories, OutputDTOIncomeCategory>(repository)
{
    protected override Dictionary<string, string> GetFailureMessages() =>
        new()
        {
            { "Categories", "Categories not found" }
        };

    protected override OutputDTOIncomeCategory MapEntityToDTO(IncomeCategory entity) =>
        new()
        {
            Id = entity.Id,
            Name = entity.Name,
            Limit = entity.Limit,
        };
}