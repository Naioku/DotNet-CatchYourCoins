using Application.DTOs.OutputDTOs.Incomes;
using Application.Requests.Queries;
using AutoMapper;
using Domain.Dashboard.Entities.Incomes;
using Domain.Interfaces.Repositories;

namespace Application.Incomes.Queries.GetAll;

public class QueryGetAllIncomes : QueryCRUDGetAll<OutputDTOIncome>;

public class HandlerGetAllIncomes(
    IRepositoryIncome repository,
    IMapper mapper)
    : HandlerCRUDGetAll<
        Income,
        QueryGetAllIncomes,
        OutputDTOIncome
    >(repository, mapper)
{
    protected override Dictionary<string, string> GetFailureMessages() =>
        new()
        {
            { "Incomes", "Incomes not found" }
        };
}