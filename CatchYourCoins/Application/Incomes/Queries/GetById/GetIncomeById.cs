using Application.DTOs.OutputDTOs.Incomes;
using Application.Requests.Queries;
using AutoMapper;
using Domain.Dashboard.Entities.Incomes;
using Domain.Interfaces.Repositories;

namespace Application.Incomes.Queries.GetById;

public class QueryGetIncomeById : QueryCRUDGetById<OutputDTOIncome>;

public class HandlerGetIncomeById(
    IRepositoryIncome repository,
    IMapper mapper)
    : HandlerCRUDGetById<
        Income,
        QueryGetIncomeById,
        OutputDTOIncome
    >(repository, mapper)
{
    protected override Dictionary<string, string> GetFailureMessages() =>
        new()
        {
            { "Income", "Income not found" }
        };
}