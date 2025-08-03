using Application.DTOs.OutputDTOs.Incomes;
using Application.Requests.Base.Queries;
using AutoMapper;
using Domain.Dashboard.Entities.Incomes;
using Domain.Interfaces.Repositories;

namespace Application.Requests.Incomes.Queries.GetById;

public class QueryGetCategoryById : QueryCRUDGetById<OutputDTOIncomeCategory>;

public class HandlerGetCategoryById(
    IRepositoryIncomeCategory repository,
    IMapper mapper)
    : HandlerCRUDGetById<
        IncomeCategory,
        QueryGetCategoryById,
        OutputDTOIncomeCategory
    >(repository, mapper)
{
    protected override Dictionary<string, string> GetFailureMessages() =>
        new()
        {
            { "Category", "Category not found" }
        };
}