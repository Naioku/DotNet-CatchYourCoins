using Application.DTOs.OutputDTOs.Incomes;
using Application.Requests.Queries;
using Domain.Dashboard.Entities.Incomes;
using Domain.Interfaces.Repositories;

namespace Application.Incomes.Queries.GetAll;

public class QueryGetAllIncomes : QueryCRUDGetAll<OutputDTOIncome>;

public class HandlerGetAllIncomes(IRepositoryIncome repository)
    : HandlerCRUDGetAll<Income, QueryGetAllIncomes, OutputDTOIncome>(repository)
{
    protected override Dictionary<string, string> GetFailureMessages() =>
        new()
        {
            { "Incomes", "Incomes not found" }
        };

    protected override OutputDTOIncome MapEntityToDTO(Income entity) =>
        new()
        {
            Id = entity.Id,
            Amount = entity.Amount,
            Date = entity.Date,
            Description = entity.Description,
            Category = entity.Category?.Name,
        };
}