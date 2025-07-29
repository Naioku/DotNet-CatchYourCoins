using Application.DTOs.Incomes;
using Application.Requests.Queries;
using Domain.Dashboard.Entities;
using Domain.Dashboard.Entities.Incomes;
using Domain.Interfaces.Repositories;

namespace Application.Incomes.Queries.GetById;

public class QueryGetIncomeById : QueryGetByIdBase<IncomeDTO>;

public class HandlerGetIncomeById(IRepositoryIncome repository)
    : HandlerCRUDGetById<Income, QueryGetIncomeById, IncomeDTO>(repository)
{
    protected override Dictionary<string, string> GetFailureMessages() =>
        new()
        {
            { "Income", "Income not found" }
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