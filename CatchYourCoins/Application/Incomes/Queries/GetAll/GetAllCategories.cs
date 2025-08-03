using Application.DTOs.OutputDTOs.Incomes;
using Application.Requests.Queries;
using AutoMapper;
using Domain.Dashboard.Entities.Incomes;
using Domain.Interfaces.Repositories;

namespace Application.Incomes.Queries.GetAll;

public class QueryGetAllCategories : QueryCRUDGetAll<OutputDTOIncomeCategory>;

public class HandlerGetAllCategories(
    IRepositoryIncomeCategory repository,
    IMapper mapper)
    : HandlerCRUDGetAll<
        IncomeCategory,
        QueryGetAllCategories,
        OutputDTOIncomeCategory
    >(repository, mapper)
{
    protected override Dictionary<string, string> GetFailureMessages() =>
        new()
        {
            { "Categories", "Categories not found" }
        };
}