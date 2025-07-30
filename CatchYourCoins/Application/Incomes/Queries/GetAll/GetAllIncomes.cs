using Application.DTOs.Incomes;
using Application.Requests.Queries;
using Domain.Dashboard.Entities.Incomes;
using Domain.Interfaces.Repositories;

namespace Application.Incomes.Queries.GetAll;

public class QueryGetAllIncomes : QueryCRUDGetAll<IncomeDTO>;

public class HandlerGetAllIncomes(IRepositoryIncome repository)
    : HandlerCRUDGetAll<Income, QueryGetAllIncomes, IncomeDTO>(repository)
{
    protected override Dictionary<string, string> GetFailureMessages() =>
        new()
        {
            { "Incomes", "Incomes not found" }
        };

    protected override IncomeDTO MapEntityToDTO(Income entity) =>
        new()
        {
            Id = entity.Id,
            Amount = entity.Amount,
            Date = entity.Date,
            Description = entity.Description,
            Category = entity.Category?.Name,
        };
}